# 在线书店系统

## 概述
该项目是一个基于 .NET 8 的在线书店系统，包含用户管理、订单处理和图书管理等核心功能。系统使用 MySQL 作为数据库，支持缓存以提高性能，使用 JWT 进行用户身份验证，并包含异常处理、日志记录和单元测试。

## 功能特点
- **用户管理**：注册、登录、查看个人信息等。
- **订单管理**：创建订单、取消订单、查询订单状态等。
- **图书管理**：添加、更新、查询、删除图书。
- **数据库**：使用 MySQL 进行数据持久化存储。
- **缓存机制**：实现缓存以增强性能。
- **JWT 身份验证**：使用 JSON Web Token 进行安全身份验证。
- **异常处理**：提供健壮的错误处理机制。
- **日志记录**：记录操作和错误日志。
- **单元测试**：为核心功能提供全面的单元测试。

## 环境要求
- **.NET 8 SDK**
- **MySQL 服务器**
- **Redis 服务器**（用于缓存）
- **开发工具**：Visual Studio 2022 或任何支持 .NET 8 的 IDE

## 本地开发环境搭建

### 1. 配置 .NET 环境
确保已安装 .NET 8 SDK，并配置好开发环境。

### 2. 配置 MySQL 数据库
运行初始化脚本以配置 MySQL 数据库： BookStore\InitDBScript\InitScript.sql


### 3. 配置 Redis
安装并运行 Redis。默认情况下，应用将使用本地的 Redis 实例 (`localhost:6379`)。

### 4. 配置应用程序
在 `appsettings.json` 中配置数据库连接字符串和 Redis 连接。

### 5. 运行应用程序
在项目根目录，运行以下命令启动应用程序：


### 6. 访问 API 文档
通过浏览器访问 `https://localhost:7092/index.html` 来查看 API 文档。
