<div align="center"><a name="readme-top"></a>

<h1>Nels.AI</h1>

Built on .NET + Vue for an AI Agent application.

[English](./README.en.md) · 简体中文

</div>

![Login](./doc/img/login.png)
![Dialog](./doc/img/agent_debug.png)

## ✨ Technology Stack

- 🌈 **Backend**: Based on the ABP backend development framework
- 🧩 **AI Framework**: semantic-kernel, kernel-memory
- ⚡ **Frontend**: vue3 + js

## Project Goals
Developing an AI Agent application based on .NET and Vue using semantic-kernel. This allows developers to quickly integrate with existing .NET projects and build AI applications rapidly.

- Note that this is an ongoing project.
- We are eagerly seeking developers proficient in Vue for collaboration.
- Future plans include knowledge base management and workflow.

## 🚀 Quick Start

### Download the Code
```bash
git clone https://github.com/nels-ai-source/nels.ai.git
```
### Start Backend Service
#### 1. Restore Database
Open "Package Manager Console", set "Nels.DbMigrator" as the startup project, and select "Nels.Aigc.EntityFrameworkCore" as the default project.

Run the command: Update Database. Note that you need to have PostgreSQL installed and modify the connection string in "appsettings".

```bash
PM> Update-Database
```

#### 2. Execute Seed Data
Run the "Nels.DbMigrator" console application.

#### 3. Start Service
Run "Nels.HttpApi.Host".

Start Frontend
#### 1. Install Dependencies
```bash
npm install
```
#### 2. Start Frontend
```bash
npm run serve
```

### Add Model Instance
1. Log in to the backend, click on the "Model" page under "Settings" in the top menu.
2. Select a model, click "Create Model Instance", and fill in the key and description.

### Create Agent
1. Log in to the backend, click on the "Agent" page in the top menu.
2. After adding a new Agent, click on the Agent card to enter the details page and set the Prompt to start chatting.

## How to Contribute
If you wish to contribute, feel free to submit a Pull Request or report a Bug.

## Open Source Projects Used
ABP
semantic-kernel
kernel-memory
SCUI