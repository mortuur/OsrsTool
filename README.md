# OsrsTool

Dit repository bevat de OsrsTool applicatie — een .NET backend met een TypeScript/React frontend.

Deze README beschrijft:
- Projectoverzicht
- Vereisten en how-to build/run
- Tests
- Git Flow setup en veelgebruikte commands (stap-voor-stap, in het Nederlands)
- Branch- en bijdrageconventies

## Projectstructuur (kort)

- `OsrsTool.server/` - ASP.NET Core Web API
- `OsrsTool.Infrastructure/` - EF Core, data access, repositories
- `OsrsTool.Domain/` - domeinmodellen, interfaces, DTOs
- `OsrsTool.client/` - React/TypeScript frontend (Vite)
- `Test/` - unit tests

## Vereisten

- Git
- .NET SDK (versie geschikt voor het project — controleer `global.json` of de projectbestanden)
- Node.js (voor de frontend)
- Optioneel: `git-flow` (AVH) voor command-line Git Flow, of gebruik de handmatige git-commando's hieronder

Op Windows kun je Git Flow installeren met Chocolatey (indien beschikbaar):

```bash
choco install gitflow-avh
```

Of gebruik Git for Windows / Git Extensions / SourceTree die Git Flow-integratie aanbieden.

## Build en run (kort)

Backend (vanuit repository root):

```bash
# Ga naar server map
cd OsrsTool.server

# Restore en build
dotnet restore
dotnet build

# Run (Development)
dotnet run
```

Frontend (vanuit `OsrsTool.client`):

```bash
cd OsrsTool.client
npm install
npm run dev
```

## Tests

Run unit tests vanuit project root:

```bash
dotnet test
```

## Git Flow — introductie en setup (Nederlands)

Git Flow is een branch-strategie die het werken met features, releases en hotfixes vereenvoudigt. Dit stuk legt uit hoe je Git Flow opzet en gebruikt in dit repository.

Belangrijke branches (conventie):
- `main` (of `master`) — productiecode
- `develop` — integratiebranch voor de volgende release
- feature/* — features in ontwikkeling
- release/* — voorbereiden van een release
- hotfix/* — fixes voor productie

Let op: in jouw repository staat momenteel een branch genaamd `README.ME`. Je kunt Git Flow gebruiken zonder de naam te hernoemen, maar het is aan te raden om een heldere hoofdbranchnaam te gebruiken (`main`). Hieronder staan twee opties om te beginnen.

### Optie A — eenvoudige start (maak `develop` aan vanaf huidige branch)

1. Zorg dat je lokaal up-to-date bent:

```bash
git fetch origin
git checkout README.ME
git pull
```

2. Maak `develop` aan en push hem:

```bash
git checkout -b develop
git push -u origin develop
```

3. Initialiseer Git Flow en kies `README.ME` als production branch wanneer daarom gevraagd wordt (of accepteer defaults als je `main`/`develop` hebt):

```bash
git flow init
```

Je kunt `-d` gebruiken om default keuzes te accepteren (alleen handig als `main` of `master` al correct bestaat):

```bash
git flow init -d
```

Als `git flow init` vraagt welke branch productie is, kun je `README.ME` invullen (of eerst de branch hernoemen naar `main` — zie Optie B).

### Optie B — hernoem huidige branch naar `main` (aanbevolen voor duidelijkheid)

> Pas op: dit verandert de naam van de hoofdbranch. Als anderen samenwerken, overleg eerst of wijzig remote branche-instellingen.

```bash
# Hernoem lokale branch
git branch -m README.ME main

# Push en zet upstream
git push origin main
git push origin --delete README.ME   # (optioneel en destructief — gebruik alleen als zeker)

# Maak develop aan
git checkout -b develop
git push -u origin develop

# Init git flow met defaults
git flow init -d
```

### Veelgebruikte Git Flow commando's

- Start een feature:

```bash
git flow feature start mijn-feature
# werk aan code
git add .
git commit -m "Begin feature: mijn-feature"
git flow feature finish mijn-feature
```

Hiermee wordt de feature gemerged in `develop` en de feature-branch lokaal verwijderd. Je moet meestal nog een `git push` naar remote doen:

```bash
git push origin develop
```

- Start een release (bijna klaar voor productie):

```bash
git flow release start 1.2.0
# bump versies / laatste checks
git flow release finish 1.2.0
```

Dit merge de release naar `main` en `develop`, tagt de release en maakt de release-branch schoon.

- Hotfix (kritische productie-fix)

```bash
git flow hotfix start fix-issue
# fix
git flow hotfix finish fix-issue
```

### Handige alternatieven (zonder git-flow)

Als je geen `git-flow` tool hebt, kun je de flow handmatig doen. Voor features:

```bash
git checkout develop
git pull
git checkout -b feature/mijn-feature
# werk en commit
git checkout develop
git merge --no-ff feature/mijn-feature
git push origin develop
```

Voor releases/production: merge develop naar main en tag:

```bash
git checkout main
git merge --no-ff develop
git tag -a v1.2.0 -m "Release 1.2.0"
git push origin main --tags
```

## Branch naming conventies (aanbevolen)

- feature/<kort-omschrijving>
- bugfix/<issue-nr>-<kort>
- hotfix/<kort>
- release/<versie>
- docs/<kort>

Gebruik korte, beschrijvende namen en verbind ze (optioneel) met issue-nummers.

## Pull requests en code review

- Open PR's van `feature/*` naar `develop`.
- Voor productie-releases: PR van `release/*` naar `main` indien gewenst door je team.
- Voeg een korte beschrijving toe, teststappen en relevante screenshots/logs.

## Contribution

1. Fork of maak een branch vanaf `develop`.
2. Volg de branch naming rules.
3. Voeg tests toe voor belangrijke wijzigingen.
4. Open een PR richting `develop` en vraag om review.

## Contact / verdere stappen

Als je wilt, kan ik:

- de README verder aanpassen (meer details per folder, commands voor Docker, CI/CD)
- een CONTRIBUTING.md toevoegen met PR-template
- een eenvoudige git alias-setup toevoegen (voor Windows bash)

Laat me weten welke toevoegingen je wilt. Als je wilt dat ik Git Flow direct in jouw repo initieer (bijv. `develop` aanmaken, of hoofdbranch hernoemen), geef het aan en ik voer de bijbehorende git-commando's uit (ik kan die hier ook voor je runnen).

---

Bedankt — laat weten welke taalstijl (Nederlands/Engels) en hoeveel detail je in de README wilt; ik pas het aan.
