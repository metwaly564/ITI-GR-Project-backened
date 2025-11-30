# إصلاح مشاكل قاعدة البيانات

## المشكلة 1: أعمدة التوصيات مفقودة
الخطأ: `Invalid column name 'Benefits'`, `'Ingredients'`, `'Rating'`, `'ReviewCount'`, `'SuitableForSkinTypes'`

هذه الأعمدة غير موجودة في قاعدة البيانات بعد إضافتها إلى نموذج Product.

## المشكلة 2: ImageUrl لا يقبل NULL
الخطأ: `Cannot insert the value NULL into column 'ImageUrl'`

العمود `ImageUrl` في قاعدة البيانات لا يقبل NULL، لكن الكود يحاول إدخال منتجات بدون صور.

## الحل السريع (SQL مباشرة)

افتح SQL Server Management Studio (SSMS) أو أي أداة SQL واتصل بقاعدة البيانات `PharmacyDB`، ثم نفذ هذا السكريبت:

```sql
USE PharmacyDB;
GO

ALTER TABLE [Products]
ADD [SuitableForSkinTypes] NVARCHAR(MAX) NULL,
    [Benefits] NVARCHAR(MAX) NULL,
    [Ingredients] NVARCHAR(MAX) NULL,
    [Rating] DECIMAL(3,2) NULL,
    [ReviewCount] INT NULL DEFAULT 0;
GO
```

أو استخدم الملف الجاهز: `Migrations\AddRecommendationFields.sql`

## إصلاح مشكلة ImageUrl

نفذ هذا السكريبت لجعل `ImageUrl` nullable:

```sql
USE PharmacyDB;
GO

ALTER TABLE [Products]
ALTER COLUMN [ImageUrl] NVARCHAR(500) NULL;
GO
```

أو استخدم الملف الجاهز: `Migrations\FixImageUrlNullable.sql`

## الحل باستخدام EF Core Migration

1. تأكد من أنك في مجلد المشروع:
```bash
cd "c:\Users\hp\Documents\GitHub\ITI-GR-Project-backened"
```

2. قم بتطبيق Migration:
```bash
dotnet ef database update
```

3. إذا لم يعمل، جرب:
```bash
dotnet ef database update AddRecommendationFieldsToProduct
```

## خطوات الإصلاح الكاملة

1. **أضف أعمدة التوصيات** (إذا لم تكن موجودة):
   - نفذ `Migrations\AddRecommendationFields.sql`

2. **أصلح ImageUrl** (مطلوب):
   - نفذ `Migrations\FixImageUrlNullable.sql`

3. **أعد تشغيل التطبيق**

## التحقق من الإصلاح

بعد تطبيق الحلول أعلاه، أعد تشغيل التطبيق وتحقق من أن:
- ✅ `/api/Home` يعمل بدون أخطاء
- ✅ `/api/Recommendation/analyze` يعمل
- ✅ لا توجد أخطاء في Console
- ✅ Seeding يعمل بنجاح

## ملاحظات

- الأعمدة الجديدة كلها `NULL` (اختيارية)، لذا لن تؤثر على البيانات الموجودة
- يمكنك ملء هذه الحقول لاحقاً للمنتجات التي تريد إضافتها للتوصيات

