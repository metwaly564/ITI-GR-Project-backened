# ITI Pharmacy Integrated System: Comprehensive Project Documentation

## 🗂️ Project Overview
The **ITI Pharmacy Management System** is an enterprise-grade, full-stack solution designed to bridge the gap between traditional pharmaceutical retail and modern healthcare AI. It provides a robust E-commerce engine for medication and skincare products, coupled with an **Expert System** that offers personalized skincare diagnostic routines.

---

## 🏗️ Backend System Architecture (.NET 8.0)

### **1. Core Infrastructure**
*   **Framework**: ASP.NET Core 8.0 (Web API).
*   **Database Engine**: Microsoft SQL Server.
*   **ORM**: Entity Framework Core 8.0 (Code-First approach).
*   **Authentication**: 
    *   **ASP.NET Core Identity**: Manages users, roles (Admin/User), and security stamps.
    *   **JWT (JSON Web Tokens)**: Stateless authentication with configurable expiry and secure signing keys.
*   **Dependency Injection**: Modular service registration for high maintainability and testability.

### **2. Data Models & Schema**
The system operates on a complex relational schema:
*   **Product & Inventory**:
    *   `Product`: Contains rich metadata including `SuitableForSkinTypes`, `Benefits`, and `Ingredients` stored as JSON for AI analysis. Supports prescription-required flags.
    *   `Category`: Hierarchical organization of products for optimized discovery.
    *   `ProductStock`: A junction table managing quantities across multiple `PharmacyBranch` locations with minimum/maximum alert levels.
*   **Sales & Logistics**:
    *   `Order`: Manages the lifecycle of a purchase (`Pending`, `Processing`, `Shipped`, `Delivered`, `Cancelled`). Tracks `PaymentStatus` and `PaymentMethod`.
    *   `OrderItem`: Line-level details of each order including historical price snapshots.
    *   `DeliveryBoy`: Personnel assigned to specific shipping zones for fulfillment.
*   **Geography & Shipping**:
    *   `City` & `Zone`: Defines delivery coverage and variable delivery fees based on location.
*   **User Engagement**:
    *   `Cart` & `Wishlist`: Persistent storage for shopping intent.
    *   `Review`: User feedback loop with numerical ratings and qualitative commentary.
    *   `UserNotification`: Log of SignalR-pushed alerts and system updates.

### **3. The AI Recommendation Engine**
The standout feature is the **Skincare Recommendation Logic**, split into two phases:
1.  **Symptom Analysis (`RecommendationService`)**:
    *   Uses rule-based heuristics to analyze text input (symptoms) and flags.
    *   Identifies conditions like Acne, Dryness, or Redness.
    *   Provides high-level routine advice and maps out "Ingredients to Avoid".
2.  **Product Scoring (`ProductRecommender`)**:
    *   Iterates through the product catalog and assigns a weighted score based on:
        *   **Skin Type match (+50 points)**.
        *   **Concern/Benefit keyword matching (+25 points per match)**.
        *   **Community Rating (weighted multiplier)**.
    *   Returns the top 12 most relevant products for the user's profile.

---

## 🎨 Frontend Architecture (Angular 19+)

### **1. Development Design Pattern**
*   **Standalone Components**: Modern Angular architecture where components manage their own dependencies (no `NgModule` bloat).
*   **Reactive UI**: Extensively uses `RxJS` for asynchronous data streams (API calls, real-time alerts).
*   **Design Tokens**: Managed via `app.css` and dedicated theme services for a premium, consistent look.

### **2. Feature Modules**
*   **Customer Journey**:
    *   **Home Component**: Dynamic sliders (Banners) and featured product grids.
    *   **Product Discovery**: Advanced filtering by category, skin type, and search terms.
    *   **Recommendation Journey**: A multi-step form that guides users through a skin diagnostic, culminating in a personalized routine page.
    *   **Checkout & Cart**: Real-time total calculations, delivery fee integration based on selected zone, and prescription upload support.
*   **Admin Dashboard (`admin/dashboard`)**:
    *   **Analytics**: Visual representation of sales, popular products, and inventory health.
    *   **Inventory Management**: CRUD interface for products with batch stock updates.
    *   **Order Control**: Interface for updating order status, assigning delivery boys, and handling cancellations.
    *   **Banner Management**: Visual editor for home page promotions.

### **3. Security & Interceptors**
*   **Admin Guard**: Protects sensitive management routes from unauthorized user access.
*   **Auth Interceptor**: Automatically attaches the JWT Bearer token to all outgoing API requests.

---

## 🛠️ Key Technical Features

### **1. Real-Time Capabilities**
*   **SignalR Integration**: The `NotificationHub` allows the server to push order updates or system broadcasts directly to the user's browser without page refreshes.

### **2. Reporting & Exports**
*   **Excel Services**: Integrated `EPPlus` library allows Admins to export comprehensive sales and inventory reports directly to `.xlsx` files.

### **3. Performance Optimizations**
*   **Response Compression**: Uses Gzip/Brotli to minimize payload size for product catalogs.
*   **AutoMapper**: Efficiently handles object-to-object mapping (Entities to DTOs) to prevent over-posting or data leakage.
*   **Lazy Loading**: Components are loaded on-demand via Angular Router to ensure fast initial page loads.

---

## � Execution & Setup Guide

### **Backend (Terminal)**
1.  Verify .NET 8.0 SDK is installed: `dotnet --version`.
2.  Update Database: `dotnet ef database update`.
3.  Launch API: `dotnet run`. (Available at `http://localhost:5062/swagger`).

### **Frontend (Terminal)**
1.  Install packages: `npm install`.
2.  Start Dev Server: `ng serve`. (Available at `http://localhost:4200`).
3.  **Note**: Ensure the Backend is running first so the Frontend can fetch initial configurations and banners.

---

## � Project Roadmap & Achievements
*   ✅ **Phase 1: Foundation**: Core Identity, Database Schema, and CRUD API.
*   ✅ **Phase 2: Commerce**: Shopping cart logic, checkout workflow, and order history.
*   ✅ **Phase 3: Intelligence**: Implementation of the `ProductRecommender` scoring engine.
*   ✅ **Phase 4: Operations**: Admin dashboard, Excel reporting, and SignalR notifications.
*   ✅ **Phase 5: UX**: Angular implementation with standalone components and reactive forms.

---
*Created as part of the ITI Graduation Project series.*
