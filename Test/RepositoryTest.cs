using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using OsrsTool.Infrastructure.Data;
using OsrsTool.Infrastructure.Repositories;
using OsrsTool.Domain.Interfaces;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace OsrsTool.Tests
{
    public partial class RepositoryTests
    {
        private readonly Mock<AppDbContext> _mockContext;

        private readonly Mock<DbSet<DummyEntity>> _mockDbSet;
        private readonly IRepository<DummyEntity> _repository;

        public RepositoryTests()
        {
            _mockContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            _mockDbSet = new Mock<DbSet<DummyEntity>>();
            _mockContext.Setup(c => c.Set<DummyEntity>()).Returns(_mockDbSet.Object);

            _repository = new Repository<DummyEntity>(_mockContext.Object);
        }

        [Test]
        public async Task AddAsync_ShouldCallAddOnDbSet()
        {
            // Arrange
            var entity = new DummyEntity { Id = 1, Name = "Test" };

            // Act
            await _repository.AddAsync(entity);

            // Assert
            _mockDbSet.Verify(d => d.AddAsync(entity, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void Update_ShouldCallUpdateOnDbSet()
        {
            // Arrange
            var entity = new DummyEntity { Id = 2, Name = "Updated" };

            // Act
            _repository.Update(entity);

            // Assert
            _mockDbSet.Verify(d => d.Update(entity), Times.Once);
        }

        [Test]
        public void Remove_ShouldCallRemoveOnDbSet()
        {
            // Arrange
            var entity = new DummyEntity { Id = 3, Name = "Remove" };

            // Act
            _repository.Remove(entity);

            // Assert
            _mockDbSet.Verify(d => d.Remove(entity), Times.Once);
        }

        [Test]
        public async Task SaveChangesAsync_ShouldCallSaveChangesOnContext()
        {
            // Act
            await _repository.SaveChangesAsync();

            // Assert
            _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnEntity()
        {
            // Arrange
            var entity = new DummyEntity { Id = 4, Name = "GetById" };
            _mockDbSet.Setup(d => d.FindAsync(It.IsAny<object[]>())).ReturnsAsync(entity);

            // Act
            var result = await _repository.GetByIdAsync(4);

            // Assert
            result.ShouldNotBeNull();
            result!.Name.ShouldBe("GetById");
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllEntities()
        {
            // Arrange
            var data = new List<DummyEntity>
            {
                new() { Id = 1, Name = "One" },
                new() { Id = 2, Name = "Two" }
            };

            var mockSet = GetQueryableMockDbSet(data);
            _mockContext.Setup(repo => repo.Set<DummyEntity>()).Returns(mockSet);

            var repo = new Repository<DummyEntity>(_mockContext.Object);

            // Act
            var result = await repo.GetAllAsync();

            // Assert
            result.Count().ShouldBe(2);
            result.ShouldContain(x => x.Name == "One");
            result.ShouldContain(x => x.Name == "Two");
        }

        [Test]
        public async Task FindAsync_ShouldReturnMatchingEntities()
        {
            // Arrange
            var data = new List<DummyEntity>
            {
                new() { Id = 1, Name = "Alpha" },
                new() { Id = 2, Name = "Beta" },
                new() { Id = 3, Name = "Alpha" }
            };

            var mockSet = GetQueryableMockDbSet(data);

            _mockContext.Setup(c => c.Set<DummyEntity>()).Returns(mockSet);

            var repo = new Repository<DummyEntity>(_mockContext.Object);

            // Act
            var result = await repo.FindAsync(x => x.Name == "Alpha");

            // Assert
            result.Count().ShouldBe(2);
            result.ShouldAllBe(x => x.Name == "Alpha");
        }

        private DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();
            var dbSet = new Mock<DbSet<T>>();

            dbSet.As<IAsyncEnumerable<T>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns((CancellationToken cancellationToken) => new TestAsyncEnumerator<T>(queryable.GetEnumerator()));

            dbSet.As<IQueryable<T>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<T>(queryable.Provider));
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>(sourceList.Add);

            return dbSet.Object;
        }

        private sealed class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
        {
            private readonly IQueryProvider _inner;

            public TestAsyncQueryProvider(IQueryProvider inner) => _inner = inner;

            public IQueryable CreateQuery(Expression expression) =>
                new TestAsyncEnumerable<TEntity>(expression);

            public IQueryable<TElement> CreateQuery<TElement>(Expression expression) =>
                new TestAsyncEnumerable<TElement>(expression);

            public object Execute(Expression expression) => _inner.Execute(expression)!;

            public TResult Execute<TResult>(Expression expression) => _inner.Execute<TResult>(expression)!;

            public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression) =>
                new TestAsyncEnumerable<TResult>(expression);

            public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken) =>
                Execute<TResult>(expression);
        }

        private sealed class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
        {
            public TestAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable)
            {
            }

            public TestAsyncEnumerable(Expression expression) : base(expression)
            {
            }

            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default) =>
                new TestAsyncEnumerator<T>(((IEnumerable<T>)this).GetEnumerator());

            IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);
        }

        private sealed class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
        {
            private readonly IEnumerator<T> _inner;

            public TestAsyncEnumerator(IEnumerator<T> inner) => _inner = inner;

            public T Current => _inner.Current;

            public ValueTask DisposeAsync()
            {
                _inner.Dispose();
                return ValueTask.CompletedTask;
            }

            public ValueTask<bool> MoveNextAsync() => new(_inner.MoveNext());
        }
    }

    public class DummyEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
