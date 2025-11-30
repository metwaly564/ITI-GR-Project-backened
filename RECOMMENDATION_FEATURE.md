# Product Recommendation Feature

This document describes the recommendation feature that has been added to the ITI GR Project, based on the Skincare AI project.

## Overview

The recommendation feature allows users to get personalized product recommendations based on their skin type, concerns, and symptoms. The system analyzes user inputs and provides:
- Identified skin conditions
- Recommended ingredients
- Ingredients to avoid
- Personalized product recommendations
- Routine advice

## Backend Changes

### New Files Created

1. **Models/Enums/SkinType.cs**
   - Enum for skin types: Normal, Dry, Oily, Combination, Sensitive

2. **DTOs/RecommendationDTOs.cs**
   - `RecommendationRequestDTO`: Input model for recommendation requests
   - `RecommendationResponseDTO`: Response model with analysis results
   - `ProductRecommendationDTO`: Product recommendation details

3. **Services/ProductRecommender.cs**
   - Scoring algorithm for product recommendations
   - Calculates product scores based on skin type compatibility and concern matching

4. **Services/IRecommendationService.cs**
   - Interface for recommendation service

5. **Services/RecommendationService.cs**
   - Main service implementing recommendation logic
   - Analyzes symptoms and generates recommendations

6. **Controllers/User/RecommendationController.cs**
   - API endpoints for recommendations:
     - `POST /api/Recommendation/analyze`: Full analysis with recommendations
     - `GET /api/Recommendation/recommendations`: Quick recommendations by skin type and concerns

### Modified Files

1. **Models/Product.cs**
   - Added optional fields for recommendation support:
     - `SuitableForSkinTypes` (JSON string): Array of compatible skin types
     - `Benefits` (JSON string): Array of product benefits
     - `Ingredients` (JSON string): Array of ingredients
     - `Rating` (decimal?): Product rating
     - `ReviewCount` (int?): Number of reviews

2. **Program.cs**
   - Registered new services:
     - `ProductRecommender`
     - `IRecommendationService` → `RecommendationService`

## Frontend Changes

### New Files Created

1. **services/recommendation-serv.ts**
   - Service for calling recommendation API
   - Types: `SkinType`, `RecommendationRequest`, `RecommendationResponse`, `ProductRecommendation`

2. **components/pages/recommendation/recommendation.ts**
   - Main recommendation component
   - Handles form submission and displays results

3. **components/pages/recommendation/recommendation.html**
   - UI for input form and results display

4. **components/pages/recommendation/recommendation.css**
   - Styling for recommendation page

### Modified Files

1. **app.routes.ts**
   - Added route: `/recommendation` → `RecommendationComponent`

2. **shared/nav/nav.html**
   - Added "Get Recommendations" link in navigation menu

3. **components/pages/home/home.html**
   - Added recommendation banner with call-to-action button

4. **components/pages/home/home.ts**
   - Added `RouterLink` import for navigation

## Database Migration Required

The Product model has been updated with new optional fields. You need to create and run a migration:

```bash
dotnet ef migrations add AddRecommendationFieldsToProduct
dotnet ef database update
```

## API Endpoints

### POST /api/Recommendation/analyze
**Request Body:**
```json
{
  "skinType": 1,
  "concerns": ["acne", "dryness"],
  "symptoms": ["itchy red patches", "flaky skin"],
  "additionalNotes": "Optional notes"
}
```

**Response:**
```json
{
  "analysisId": "guid",
  "skinType": 1,
  "identifiedConditions": ["Acne", "Dry Skin"],
  "analysisSummary": "Based on your symptoms...",
  "recommendedIngredients": ["Salicylic Acid", "Hyaluronic Acid"],
  "ingredientsToAvoid": ["Heavy Oils"],
  "recommendedProducts": [...],
  "routineAdvice": "• Cleanse twice daily...",
  "createdAt": "2025-01-20T..."
}
```

### GET /api/Recommendation/recommendations
**Query Parameters:**
- `skinType`: 1-5 (SkinType enum value)
- `concerns`: Array of concern strings

**Response:**
Array of `ProductRecommendationDTO` objects

## Usage

1. Users navigate to `/recommendation` page
2. Fill in skin type, concerns, and symptoms
3. Submit form to get personalized recommendations
4. View recommended products with reasons
5. Click on products to view details

## Notes

- The recommendation algorithm uses a scoring system based on:
  - Skin type compatibility (30 points)
  - Concern matching (20 points per match)
  - Product rating (rating * 2 points)
  - Penalty for incompatible skin types (-50 points)

- Products need to have recommendation fields populated in the database for best results
- The system works with existing products even if recommendation fields are null (will return all products with basic scoring)

