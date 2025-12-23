# 🏢 Projet : Système de gestion commerciale et de stock — GestionStock

## 🌍 Contexte  
Avec la digitalisation croissante des entreprises, la gestion manuelle des produits, du stock, des fournisseurs et des factures devient inefficace et source d’erreurs.  
Ce projet propose une application web permettant de **centraliser la gestion commerciale**, d’assurer le suivi du stock en temps réel et de sécuriser l’accès aux fonctionnalités selon le rôle des utilisateurs.

La solution vise à améliorer l’organisation interne de l’entreprise et à faciliter la prise de décision grâce à une gestion structurée et fiable.

---

## ❗ Problématique  
Les systèmes traditionnels de gestion présentent plusieurs limites :

- 📦 Difficulté de suivi précis du stock.
- 🧾 Gestion manuelle des factures et des mouvements de stock.
- 👥 Absence de séparation claire des rôles (administrateur / employé).
- 🔒 Sécurité insuffisante des comptes utilisateurs.
- 📉 Manque de visibilité globale sur l’activité commerciale.

Ces contraintes entraînent une perte de temps, des erreurs fréquentes et des risques sur la sécurité des données.

---

## 🎯 Objectifs  

- 📦 Gérer les **produits** (ajout, modification, suppression).
- 🗂️ Gérer les **catégories** de produits.
- 🚚 Gérer les **fournisseurs**.
- 👥 Gérer les **utilisateurs** avec des rôles distincts (Admin / Employee).
- 🧾 Créer et gérer les **factures** et leurs détails.
- 🔄 Suivre les **mouvements de stock** (entrées / sorties).
- 📊 Fournir des **tableaux de bord** adaptés selon le rôle.
- 🔐 Mettre en place une **authentification sécurisée**.
- 🌐 Proposer une interface web claire et responsive.

---

## 🛠️ Technologies utilisées  

- **Langage principal** : C#  
- **Framework Backend** : ASP.NET Core MVC  
- **ORM** : Entity Framework Core  
- **Base de données** : SQLite  
- **Authentification** : Cookie Authentication + Claims  
- **Frontend** : Razor Pages, HTML5, CSS, Bootstrap  
- **Gestion des styles** : Tailwind CSS  
- **Outils** : Visual Studio, Git, GitHub  

---

## 🔐 Authentification & Sécurité  

- Hachage sécurisé des mots de passe.
- Authentification par cookies.
- Gestion des rôles via Claims (Admin / Employee).
- Protection CSRF avec `ValidateAntiForgeryToken`.
- Accès restreint aux fonctionnalités selon le rôle utilisateur.

---

## 📌 Architecture du projet (MVC)

- **Models** : Représentent les entités métiers et la structure de la base de données.
- **Views** : Interfaces utilisateur développées avec Razor (`.cshtml`).
- **Controllers** : Gestion des requêtes HTTP et de la logique applicative.

Cette architecture assure une séparation claire des responsabilités et facilite la maintenance et l’évolution du projet.

---

## 📌 Structure du Projet  

```bash
Gestion/
│
├── Controllers/                      # Contrôleurs MVC
│   ├── AccountController.cs          # Authentification (Login, Logout)
│   ├── ProductController.cs          # Gestion des produits
│   ├── CategoryController.cs         # Gestion des catégories
│   ├── SupplierController.cs         # Gestion des fournisseurs
│   ├── EmployeeController.cs         # Gestion des employés
│   ├── UserController.cs             # Gestion des utilisateurs
│   └── HomeController.cs             # Dashboards (Admin / Employee)
│
├── Models/                           # Modèles métiers
│   ├── AppDbContext.cs               # Contexte EF Core
│   ├── User.cs
│   ├── Product.cs
│   ├── Category.cs
│   ├── Supplier.cs
│   ├── Client.cs
│   ├── Invoice.cs
│   ├── InvoiceDetail.cs
│   ├── StockMovement.cs
│   └── ViewModels (Login, Dashboard, etc.)
│
├── Data/                             # Accès aux données et migrations
│
├── Helpers/                          # Classes utilitaires (PasswordHasher)
│
├── Views/                            # Interfaces utilisateur
│   ├── Account/                      # Login, AccessDenied
│   ├── Product/                      # CRUD Produits + Stock
│   ├── Category/                     # CRUD Catégories
│   ├── Supplier/                     # CRUD Fournisseurs
│   ├── Employee/                     # CRUD Employés
│   ├── Home/                         # Dashboards
│   └── Shared/                       # Layout, erreurs, scripts partagés
│
├── wwwroot/                          # Fichiers statiques (CSS, JS, Bootstrap)
│
├── appsettings.json                  # Configuration
├── Program.cs                        # Point d’entrée
├── Gestion.csproj                    # Configuration du projet
└── Gestion.sln                       # Solution Visual Studio
