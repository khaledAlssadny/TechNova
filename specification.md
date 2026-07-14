# TechNova — Electronics eCommerce Platform
## Project Specification & Enhancement Roadmap

> **Living Document** — Updated after each phase is completed and verified.

---

## 📌 Project Overview

**Current Name in UI:** `Electro - HTML Ecommerce Template` → **Rename to: `TechNova`**

**Tech Stack:**
- ASP.NET Core MVC (.NET 8)
- Entity Framework Core + SQL Server
- ASP.NET Identity (Authentication/Authorization)
- SignalR (Real-time Comments)
- Bootstrap 3 + jQuery (Frontend)

**Team Members (as seen in code comments):** Omar, Saeed, AbdElraheem, Hoda Aswan

---

## 🔍 Current State Audit

### ✅ What Works
- Product listing with pagination (6 per page)
- Product search by name (redirects to first match)
- Product detail page with related products and comments (SignalR)
- Category filtering with AJAX partial views
- Price range filtering
- Cart (add item, view cart, update quantity)
- User registration with email confirmation flow
- Login / Logout
- Forgot password / Reset password via email
- Admin dashboard (Users, Admins, Products, Categories, Orders, Shipments)
- Checkout flow: Cart → Order → Shipment creation
- My Account page (edit info, change password, view shipments)
- Role-based authorization (Admin / User)

### ❌ Issues and Bugs Found

#### Critical Bugs
1. **AccountController.cs line 103**: On normal registration (isAdmin = false), user is still added to "Admin" role instead of "User" role.
2. **OrderController.cs line 193**: The checkout incorrectly checks against ALL carts instead of the user's cart.
3. **ProductController.cs line 276**: Image upload on Update is not null-safe — crashes if user does not upload a new image.
4. **Cart dropdown in header** (_BlankLayout.cshtml, _IndexLayout.cshtml): Shows hardcoded fake data (product01.png, $980, qty: 3). Not connected to real cart data.
5. **Search** (ProductController.cs line 156): If no product matches, id = 0 and redirects to a nonexistent product, returning a blank page.
6. **_BlankLayout.cshtml line 26**: FontAwesome 5 CDN has a broken integrity hash (sha512-XXX), breaking icon loading in some browsers.

#### UI/UX Issues
7. **Branding**: Title tag still says "Electro - HTML Ecommerce Template" in all layouts.
8. **Navigation**: Category links in nav (asp-route-Id="1") are hardcoded IDs — will break if DB categories are reordered.
9. **Cart quantity badge** in header always shows "3" — static, not dynamic.
10. **Hot Deal countdown** on homepage is static HTML — no JavaScript timer, no real deal product.
11. **Wishlist button** is disabled (commented out in _BlankLayout), yet still visible in _IndexLayout.
12. **"Add to compare" and "Quick view"** buttons are present but have no functionality.
13. **Footer links** are all href="#" (dead links).
14. **Homepage** (_IndexLayout) has all hardcoded placeholder product cards — not populated from DB.
15. **Dashboard typo**: Directory named "Dashbourd" (misspelling of "Dashboard") appears throughout controllers and views.

#### Code Quality Issues
16. **Program.cs**: Several services registered twice (CartService, CartItemService, CartRepository, CartItemRepository — lines 59–62 and 94–98).
17. **AccountController.cs**: Imports unused namespaces (BlazorIdentity code-gen references).
18. **Password policy** (Program.cs lines 72–76): All security requirements disabled — must be re-enabled before production.
19. **No global error handling** for production.
20. **Session middleware ordering**: app.UseSession() is called after app.UseAuthorization() — should be before UseRouting to be safe.

---

## 🗺️ Enhancement Phases

---

### PHASE 1 — Branding and Critical Bug Fixes ← START HERE
**Goal:** Fix crashes and rename the app. Safe, no structural changes.

#### Tasks
- [ ] Rename all title tags from "Electro - HTML Ecommerce Template" to "TechNova"
- [ ] Update logo text/image in all layouts to say "TechNova"
- [ ] Fix AccountController register bug: normal users must be added to "User" role, not "Admin"
- [ ] Fix FontAwesome CDN integrity hash in _BlankLayout.cshtml
- [ ] Fix Search: if no results found, redirect to products list with a flash message
- [ ] Fix product Update: guard against null image upload (keep existing image if no new one uploaded)
- [ ] Fix duplicate service registrations in Program.cs
- [ ] Fix navbar logo href from "#" to Home action

**Verification:**
- Register a new user → confirm they are a User, not Admin
- Search for nonexistent product → confirm graceful redirect
- Update product without changing image → confirm no crash
- App loads without icon errors in browser console

---

### PHASE 2 — UI Rebranding and Visual Overhaul
**Goal:** Make the UI look professional and branded. Change the visual identity.

#### Tasks
- [ ] Design and apply TechNova logo
- [ ] Update color scheme: replace generic red/orange with modern dark/electric blue palette
- [ ] Upgrade Google Font from Montserrat to Inter or Outfit
- [ ] Replace hardcoded homepage product cards with dynamic server-side rendering from DB
- [ ] Make cart header dropdown show real cart items
- [ ] Fix cart quantity badge to show actual item count
- [ ] Fix Hot Deal section to either show a real product or remove it
- [ ] Remove/hide non-functional "Add to Wishlist", "Add to Compare", "Quick View" buttons
- [ ] Add "Out of Stock" badge when product Quantity = 0

**Files to modify:**
- Views/Shared/_IndexLayout.cshtml
- Views/Shared/_BlankLayout.cshtml
- wwwroot/css/style.css
- Views/Home/Index.cshtml

---

### PHASE 3 — Functional Enhancements
**Goal:** Make all core flows production-ready and user-friendly.

#### Tasks
- [ ] Fix checkout cart user-scoping (use logged-in user's cart, not first cart in DB)
- [ ] Add [Authorize] decorator to checkout action (currently unprotected)
- [ ] Dynamic navigation categories (load from DB, not hardcoded IDs)
- [ ] Add proper "Search Results" page (instead of redirecting to single product)
- [ ] Add quantity control on cart page (increment/decrement buttons)
- [ ] Add "Remove from cart" button functionality
- [ ] Add order status tracking (Pending, Processing, Shipped, Delivered)
- [ ] Add Status field to Order model and migration
- [ ] Link footer navigation links to real pages/actions

---

### PHASE 4 — Admin Dashboard Improvements
**Goal:** Make the admin dashboard fully functional and polished.

#### Tasks
- [ ] Fix DashbourdController typo — rename properly while keeping backward compat routes
- [ ] Add product image preview on insert/update forms
- [ ] Add confirmation dialog for delete operations
- [ ] Add order status management (admin can change order status)
- [ ] Add sales summary/stats cards on dashboard home
- [ ] Add low-stock alerts (products with Quantity < 5)
- [ ] Add pagination to admin product/user lists

---

### PHASE 5 — Security and Production Readiness
**Goal:** Harden the app before deployment.

#### Tasks
- [ ] Re-enable password complexity requirements (Program.cs)
- [ ] Add [Authorize] to all cart operations
- [ ] Add anti-forgery tokens to all POST forms that are missing them
- [ ] Add global exception middleware with user-friendly error pages
- [ ] Review all commented-out [Authorize] decorators and re-enable where needed
- [ ] Remove development-only code and TODO comments from production path
- [ ] Add input validation to editAccountInfo (no duplicate username check on frontend)
- [ ] Move sensitive config (connection strings, mail settings) to environment variables

---

### PHASE 6 — Performance and SEO
**Goal:** Optimize load time and discoverability.

#### Tasks
- [ ] Add meta descriptions to all pages
- [ ] Add OpenGraph tags for product detail pages
- [ ] Add caching for category list (rarely changes)
- [ ] Lazy-load product images
- [ ] Add alt text to all product images
- [ ] Minify custom CSS in production
- [ ] Add robots.txt and sitemap.xml

---

## 📂 File Reference Map

| Area | Key Files |
|------|-----------|
| Main layout (inner pages) | Views/Shared/_BlankLayout.cshtml |
| Homepage layout | Views/Shared/_IndexLayout.cshtml |
| Product store layout | Views/Shared/_StoreLayout.cshtml |
| Product detail layout | Views/Shared/_ProductLayout.cshtml |
| Cart layout | Views/Shared/_CartLayout.cshtml |
| Checkout layout | Views/Shared/_CheckoutLayout.cshtml |
| Dashboard layout | Views/Shared/_DashbourdLayout.cshtml |
| Global CSS | wwwroot/css/style.css |
| Dependency injection | Program.cs |
| User management | Controllers/AccountController.cs |
| Products | Controllers/ProductController.cs |
| Orders | Controllers/OrderController.cs |
| Admin dashboard | Controllers/DashbourdController.cs |
| Data models | Models/ |

---

## 📋 Naming Convention — Chosen Brand Name

| | Current | New |
|--|---------|-----|
| App Title | Electro - HTML Ecommerce Template | TechNova |
| HTML title tag | Electro - HTML Ecommerce Template | TechNova — Premium Electronics |
| Logo | (image) | TechNova |
| Footer brand | Colorlib template | TechNova Electronics |

---

## 🔄 Change Log

| Date | Phase | Description |
|------|-------|-------------|
| 2026-07-14 | Spec | Initial specification created after full project audit |
