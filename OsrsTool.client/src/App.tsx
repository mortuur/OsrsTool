function App() {
    return (
        <div className="min-h-screen flex items-center justify-center bg-gray-900">
            <p className="text-red-500">Testkleur</p>

            <div
                className="p-4 mb-4 text-sm !text-green-300 rounded-lg bg-green-900 border border-green-400"
                role="alert">
                <span className="font-medium">Success!</span> Tailwind werkt 🎉
            </div>
        </div>
    );
}

export default App;
