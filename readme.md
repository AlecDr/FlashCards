<p align="center">
    <h1 align="center">FLASHCARDS 📚</h1>
</p>

<p align="center">
	<img src="https://img.shields.io/github/license/AlecDr/FlashCards?style=flat&logo=opensourceinitiative&logoColor=white&color=0080ff" alt="license">
	<img src="https://img.shields.io/github/last-commit/AlecDr/FlashCards?style=flat&logo=git&logoColor=white&color=0080ff" alt="last-commit">
	<img src="https://img.shields.io/github/languages/top/AlecDr/FlashCards?style=flat&color=0080ff" alt="repo-top-language">
	<img src="https://img.shields.io/github/languages/count/AlecDr/FlashCards?style=flat&color=0080ff" alt="repo-language-count">
</p>
<p align="center">
	</p>

<br>

##### 🔗 Table of Contents

- [📍 Overview](#-overview)
- [👾 Features](#-features)
- [📂 Repository Structure](#-repository-structure)
- [🧩 Modules](#-modules)
- [🚀 Getting Started](#-getting-started)
    - [🔖 Prerequisites](#-prerequisites)
    - [📦 Installation](#-installation)
    - [🤖 Usage](#-usage)
- [📌 Project Roadmap](#-project-roadmap)
- [🤝 Contributing](#-contributing)
- [🎗 License](#-license)
- [🙌 Acknowledgments](#-acknowledgments)

---

## 📍 Overview

Welcome to the **FlashCards** project! This .NET application is designed to help users create, manage, and study flashcards. Whether you're preparing for an exam or just looking to keep your knowledge sharp, this tool will make your study sessions more efficient and enjoyable.

---

## 👾 Features

- **Create and Manage Flashcards**: Easily create new flashcards, organize them into stacks, and edit or delete them as needed.
- **Study Sessions**: Conduct study sessions by reviewing your flashcards, tracking your progress, and focusing on areas that need improvement.
- **Database Integration**: Stores flashcards in a database, making it easy to retrieve and manage large sets of cards.
- **Console-Based Interface**: User-friendly console interface for navigating through different menus and options.

### Design Patterns 🎨

This project leverages key architectural patterns to ensure maintainability and scalability:

- **Singleton Pattern**: Ensures a single instance of the database connection throughout the application.
- **DTOs (Data Transfer Objects)**: Used to encapsulate data and send it between different layers of the application.
- **DAOs (Data Access Objects)**: Abstracts and encapsulates all access to the data source, providing a clear separation between the business logic and database access code.


---

## 📂 Repository Structure

```sh
└── FlashCards/
    ├── App.config
    ├── App.config.example
    ├── Daos
    │   ├── CardDao.cs
    │   ├── StackDao.cs
    │   ├── StudySessionAnswerDao.cs
    │   └── StudySessionDao.cs
    ├── Dtos
    │   ├── Card
    │   ├── Stack
    │   ├── StudySession
    │   └── StudySessionAnswer
    ├── Enums
    │   └── MenuType.cs
    ├── FlashCards.csproj
    ├── FlashCards.sln
    ├── Helpers
    │   ├── ConsoleHelper.cs
    │   ├── DatabaseHelper.cs
    │   ├── FlashCardsHelper.cs
    │   └── ValidationHelper.cs
    ├── LICENSE.txt
    ├── Menus
    │   ├── Interfaces
    │   ├── MainMenu.cs
    │   ├── ManageCardsMenu.cs
    │   ├── ManageStacksMenu.cs
    │   └── StudySessionsMenu.cs
    └── Program.cs
```

---

## 🧩 Modules

<details closed><summary>.</summary>

| File | Summary |
| --- | --- |
| [FlashCards.sln](https://github.com/AlecDr/FlashCards/blob/main/FlashCards.sln) | <code>❯ REPLACE-ME</code> |
| [App.config.example](https://github.com/AlecDr/FlashCards/blob/main/App.config.example) | <code>❯ REPLACE-ME</code> |
| [LICENSE.txt](https://github.com/AlecDr/FlashCards/blob/main/LICENSE.txt) | <code>❯ REPLACE-ME</code> |
| [App.config](https://github.com/AlecDr/FlashCards/blob/main/App.config) | <code>❯ REPLACE-ME</code> |
| [Program.cs](https://github.com/AlecDr/FlashCards/blob/main/Program.cs) | <code>❯ REPLACE-ME</code> |
| [FlashCards.csproj](https://github.com/AlecDr/FlashCards/blob/main/FlashCards.csproj) | <code>❯ REPLACE-ME</code> |

</details>

<details closed><summary>Menus</summary>

| File | Summary |
| --- | --- |
| [ManageCardsMenu.cs](https://github.com/AlecDr/FlashCards/blob/main/Menus/ManageCardsMenu.cs) | <code>❯ REPLACE-ME</code> |
| [MainMenu.cs](https://github.com/AlecDr/FlashCards/blob/main/Menus/MainMenu.cs) | <code>❯ REPLACE-ME</code> |
| [StudySessionsMenu.cs](https://github.com/AlecDr/FlashCards/blob/main/Menus/StudySessionsMenu.cs) | <code>❯ REPLACE-ME</code> |
| [ManageStacksMenu.cs](https://github.com/AlecDr/FlashCards/blob/main/Menus/ManageStacksMenu.cs) | <code>❯ REPLACE-ME</code> |

</details>

<details closed><summary>Menus.Interfaces</summary>

| File | Summary |
| --- | --- |
| [Menu.cs](https://github.com/AlecDr/FlashCards/blob/main/Menus/Interfaces/Menu.cs) | <code>❯ REPLACE-ME</code> |

</details>

<details closed><summary>Dtos.Card</summary>

| File | Summary |
| --- | --- |
| [CardStoreDTO.cs](https://github.com/AlecDr/FlashCards/blob/main/Dtos/Card/CardStoreDTO.cs) | <code>❯ REPLACE-ME</code> |
| [CardUpdateDTO.cs](https://github.com/AlecDr/FlashCards/blob/main/Dtos/Card/CardUpdateDTO.cs) | <code>❯ REPLACE-ME</code> |
| [CardShowDTO.cs](https://github.com/AlecDr/FlashCards/blob/main/Dtos/Card/CardShowDTO.cs) | <code>❯ REPLACE-ME</code> |
| [CardPromptDTO.cs](https://github.com/AlecDr/FlashCards/blob/main/Dtos/Card/CardPromptDTO.cs) | <code>❯ REPLACE-ME</code> |

</details>

<details closed><summary>Dtos.StudySession</summary>

| File | Summary |
| --- | --- |
| [StudySessionUpdateDTO.cs](https://github.com/AlecDr/FlashCards/blob/main/Dtos/StudySession/StudySessionUpdateDTO.cs) | <code>❯ REPLACE-ME</code> |
| [StudySessionStoreDTO.cs](https://github.com/AlecDr/FlashCards/blob/main/Dtos/StudySession/StudySessionStoreDTO.cs) | <code>❯ REPLACE-ME</code> |
| [StudySessionShowDTO.cs](https://github.com/AlecDr/FlashCards/blob/main/Dtos/StudySession/StudySessionShowDTO.cs) | <code>❯ REPLACE-ME</code> |

</details>

<details closed><summary>Dtos.Stack</summary>

| File | Summary |
| --- | --- |
| [StackStoreDTO.cs](https://github.com/AlecDr/FlashCards/blob/main/Dtos/Stack/StackStoreDTO.cs) | <code>❯ REPLACE-ME</code> |
| [StackUpdateDTO.cs](https://github.com/AlecDr/FlashCards/blob/main/Dtos/Stack/StackUpdateDTO.cs) | <code>❯ REPLACE-ME</code> |
| [StackShowDTO.cs](https://github.com/AlecDr/FlashCards/blob/main/Dtos/Stack/StackShowDTO.cs) | <code>❯ REPLACE-ME</code> |
| [StackPromptDTO.cs](https://github.com/AlecDr/FlashCards/blob/main/Dtos/Stack/StackPromptDTO.cs) | <code>❯ REPLACE-ME</code> |

</details>

<details closed><summary>Dtos.StudySessionAnswer</summary>

| File | Summary |
| --- | --- |
| [StudySessionAnswerStoreDTO.cs](https://github.com/AlecDr/FlashCards/blob/main/Dtos/StudySessionAnswer/StudySessionAnswerStoreDTO.cs) | <code>❯ REPLACE-ME</code> |
| [StudySessionAnswerPromptDTO.cs](https://github.com/AlecDr/FlashCards/blob/main/Dtos/StudySessionAnswer/StudySessionAnswerPromptDTO.cs) | <code>❯ REPLACE-ME</code> |

</details>

<details closed><summary>Enums</summary>

| File | Summary |
| --- | --- |
| [MenuType.cs](https://github.com/AlecDr/FlashCards/blob/main/Enums/MenuType.cs) | <code>❯ REPLACE-ME</code> |

</details>

<details closed><summary>Daos</summary>

| File | Summary |
| --- | --- |
| [StackDao.cs](https://github.com/AlecDr/FlashCards/blob/main/Daos/StackDao.cs) | <code>❯ REPLACE-ME</code> |
| [StudySessionDao.cs](https://github.com/AlecDr/FlashCards/blob/main/Daos/StudySessionDao.cs) | <code>❯ REPLACE-ME</code> |
| [CardDao.cs](https://github.com/AlecDr/FlashCards/blob/main/Daos/CardDao.cs) | <code>❯ REPLACE-ME</code> |
| [StudySessionAnswerDao.cs](https://github.com/AlecDr/FlashCards/blob/main/Daos/StudySessionAnswerDao.cs) | <code>❯ REPLACE-ME</code> |

</details>

<details closed><summary>Helpers</summary>

| File | Summary |
| --- | --- |
| [FlashCardsHelper.cs](https://github.com/AlecDr/FlashCards/blob/main/Helpers/FlashCardsHelper.cs) | <code>❯ REPLACE-ME</code> |
| [ValidationHelper.cs](https://github.com/AlecDr/FlashCards/blob/main/Helpers/ValidationHelper.cs) | <code>❯ REPLACE-ME</code> |
| [ConsoleHelper.cs](https://github.com/AlecDr/FlashCards/blob/main/Helpers/ConsoleHelper.cs) | <code>❯ REPLACE-ME</code> |
| [DatabaseHelper.cs](https://github.com/AlecDr/FlashCards/blob/main/Helpers/DatabaseHelper.cs) | <code>❯ REPLACE-ME</code> |

</details>

---

## 🚀 Getting Started

### 🔖 Prerequisites

**CSharp**: `version 8.0 or higher`

### 📦 Installation

Build the project from source:

1. Clone the FlashCards repository:
```sh
❯ git clone https://github.com/AlecDr/FlashCards
```

2. Navigate to the project directory:
```sh
❯ cd FlashCards
```

3. Install the required dependencies:
```sh
❯ dotnet build
```

### 🤖 Usage

To run the project, execute the following command:

```sh
❯ dotnet run
```

---

## 📌 Project [Roadmap](https://www.thecsharpacademy.com/project/14/flashcards)

- [X] This is an application where the users will create Stacks of Flashcards.
- [X] You'll need two different tables for stacks and flashcards. The tables should be linked by a foreign key.
- [X] Stacks should have an unique name.
- [X] Every flashcard needs to be part of a stack. If a stack is deleted, the same should happen with the flashcard.
- [X] You should use DTOs to show the flashcards to the user ~~without the Id of the stack it belongs to~~.
- [X] When showing a stack to the user, the flashcard Ids should always start with 1 without gaps between them. If you have 10 cards and number 5 is deleted, the table should show Ids from 1 to 9.
- [X] After creating the flashcards functionalities, create a "Study Session" area, where the users will study the stacks. All study sessions should be stored, with date and score.
- [X] The study and stack tables should be linked. If a stack is deleted, it's study sessions should be deleted.
- [ ] The project should contain a call to the study table so the users can see all their study sessions. This table receives insert calls upon each study session, but there shouldn't be ~~update and~~ delete calls to it.
---

## 🤝 Contributing

Contributions are welcome! Here are several ways you can contribute:

- **[Report Issues](https://github.com/AlecDr/FlashCards/issues)**: Submit bugs found or log feature requests for the `FlashCards` project.
- **[Submit Pull Requests](https://github.com/AlecDr/FlashCards/blob/main/CONTRIBUTING.md)**: Review open PRs, and submit your own PRs.
- **[Join the Discussions](https://github.com/AlecDr/FlashCards/discussions)**: Share your insights, provide feedback, or ask questions.

<details closed>
<summary>Contributing Guidelines</summary>

1. **Fork the Repository**: Start by forking the project repository to your github account.
2. **Clone Locally**: Clone the forked repository to your local machine using a git client.
   ```sh
   git clone https://github.com/AlecDr/FlashCards
   ```
3. **Create a New Branch**: Always work on a new branch, giving it a descriptive name.
   ```sh
   git checkout -b new-feature-x
   ```
4. **Make Your Changes**: Develop and test your changes locally.
5. **Commit Your Changes**: Commit with a clear message describing your updates.
   ```sh
   git commit -m 'Implemented new feature x.'
   ```
6. **Push to github**: Push the changes to your forked repository.
   ```sh
   git push origin new-feature-x
   ```
7. **Submit a Pull Request**: Create a PR against the original project repository. Clearly describe the changes and their motivations.
8. **Review**: Once your PR is reviewed and approved, it will be merged into the main branch. Congratulations on your contribution!
</details>

<details closed>
<summary>Contributor Graph</summary>
<br>
<p align="left">
   <a href="https://github.com{/AlecDr/FlashCards/}graphs/contributors">
      <img src="https://contrib.rocks/image?repo=AlecDr/FlashCards">
   </a>
</p>
</details>

---

## 🎗 License

This project is protected under the [MIT](https://choosealicense.com/licenses/mit/) License. For more details, refer to the [LICENSE](https://github.com/AlecDr/FlashCards/blob/master/LICENSE.txt) file.

---

## 🙌 Acknowledgments

- Idea taken from [C# Academy](https://www.thecsharpacademy.com/), nice location to learn C#.

---