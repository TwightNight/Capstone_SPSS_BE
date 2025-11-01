namespace SPSS.Shared.Constants
{
    public static class ExceptionMessageConstants
    {
        public static class Brand
        {
            // DTO validation messages
            public const string NameRequired = "Brand name is required.";
            public const string NameTooLong = "Brand name cannot be longer than 100 characters.";

            public const string TitleRequired = "Title is required.";
            public const string TitleTooLong = "Title cannot be longer than 200 characters.";

            public const string DescriptionRequired = "Description is required.";
            public const string DescriptionTooLong = "Description cannot be longer than 500 characters.";

            public const string ImageUrlRequired = "Image URL is required.";
            public const string ImageUrlTooLong = "Image URL cannot be longer than 500 characters.";
            public const string ImageUrlInvalid = "A valid URL is required for the image.";

            public const string CountryIdRequired = "Country ID is required.";

            // Service / repository messages
            public const string BrandDataNull = "Brand data cannot be null.";
            public const string NotFound = "Brand with ID {0} not found.";
            public const string FailedToCreate = "Failed to create brand: {0}";
            public const string FailedToUpdate = "Failed to update brand: {0}";
            public const string FailedToDelete = "Failed to delete brand: {0}";
            public const string NameAlreadyExists = "Brand name '{0}' already exists.";
            public const string InUseByProducts = "Cannot delete brand. It is currently in use by one or more products.";
        }

        public static class Country
        {
            // DTO validation messages
            public const string CountryCodeRequired = "Country code is required.";
            public const string CountryCodeTooLong = "Country code cannot be longer than 10 characters.";
            public const string CountryNameRequired = "Country name is required.";
            public const string CountryNameTooLong = "Country name cannot be longer than 100 characters.";

            // Service / repository messages
            public const string CountryDataNull = "Country data cannot be null.";
            public const string NotFound = "Country with ID {0} not found.";
            public const string CodeAlreadyExists = "Country code '{0}' already exists.";
            public const string NameAlreadyExists = "Country name '{0}' already exists.";
            public const string InUseByAddresses = "Cannot delete country. It is currently in use by one or more addresses.";
            public const string FailedToCreate = "Failed to create country: {0}";
            public const string FailedToUpdate = "Failed to update country: {0}";
            public const string FailedToDelete = "Failed to delete country: {0}";
        }

        public static class SkinCondition
        {
            // DTO validation messages
            public const string NameRequired = "Skin condition name is required.";
            public const string NameTooLong = "Name cannot be longer than 200 characters.";
            public const string DescriptionTooLong = "Description cannot be longer than 1000 characters.";
            public const string SeverityRange = "Severity level must be between 1 and 10.";

            // Service / repository messages (kept / existing)
            public const string SkinConditionDataNull = "Skin condition data cannot be null.";
            public const string NotFound = "Skin condition with ID {0} not found.";
            public const string FailedToSave = "Failed to save skin condition: {0}";
            public const string FailedToDelete = "Failed to delete skin condition: {0}";
            public const string NameAlreadyExists = "Skin condition name '{0}' already exists.";
            public const string InUseByUser = "Cannot delete skin condition. It is currently in use.";
        }

        public static class SkinType
        {
            // DTO validation messages
            public const string NameRequired = "Skin type name is required.";
            public const string NameTooLong = "Name cannot be longer than 255 characters.";
            public const string DescriptionTooLong = "Description cannot be longer than 500 characters.";

            // Service / repository messages (existing)
            public const string SkinTypeDataNull = "SkinType data cannot be null.";
            public const string NotFound = "SkinType with ID {0} not found.";
            public const string FailedToCreate = "Failed to create skin type: {0}";
            public const string FailedToUpdate = "Failed to update skin type: {0}";
            public const string FailedToDelete = "Failed to delete skin type: {0}";
            public const string NameAlreadyExists = "Skin type name '{0}' already exists.";
            public const string InUseByProduct = "Cannot delete skin type. It is currently in use by one or more products.";
        }

        public static class CancelReason
        {
            // DTO validation messages
            public const string DescriptionRequired = "Description is required.";
            public const string DescriptionTooLong = "Description cannot be longer than 500 characters.";

            public const string RefundRateRequired = "Refund rate is required.";
            public const string RefundRateOutOfRange = "Refund rate must be between 0 and 100.";

            // Service / repository messages
            public const string CancelReasonDataNull = "Cancel reason data cannot be null.";
            public const string NotFound = "Cancel reason with ID {0} not found.";
            public const string FailedToCreate = "Failed to create cancel reason: {0}";
            public const string FailedToUpdate = "Failed to update cancel reason: {0}";
            public const string FailedToDelete = "Failed to delete cancel reason: {0}";
            public const string InUseByOrders = "Cannot delete cancel reason. It is currently used by one or more orders.";
        }

        public static class CartItem
        {
            // DTO validation messages
            public const string UserIdRequired = "User ID is required.";
            public const string ProductIdRequired = "Product ID is required.";
            public const string QuantityRequired = "Quantity is required.";
            public const string QuantityAtLeastOne = "Quantity must be at least 1.";

            // Service / repository messages (ví dụ)
            public const string CartItemDataNull = "Cart item data cannot be null.";
            public const string NotFound = "Cart item with ID {0} not found.";
            public const string FailedToCreate = "Failed to create cart item: {0}";
            public const string FailedToUpdate = "Failed to update cart item: {0}";
            public const string FailedToDelete = "Failed to delete cart item: {0}";
            public const string InUseByOrders = "Cannot delete cart item. It is currently associated with an order.";
        }

        public static class Order
        {
            // DTO validation messages
            public const string CancelReasonIdRequired = "The cancellation reason ID is required.";
            public const string PaymentMethodIdRequired = "The payment method ID is required.";
            public const string AddressIdRequired = "The shipping address ID is required.";
            public const string OrderMustContainAtLeastOne = "The order must contain at least one item.";
            public const string StatusRequired = "Order status is required.";
            public const string StatusTooLong = "Status cannot be longer than 100 characters.";

            // Service / repository messages
            public const string OrderDataNull = "Order data cannot be null.";
            public const string NotFound = "Order with ID {0} not found.";
            public const string FailedToCreate = "Failed to create order: {0}";
            public const string FailedToUpdate = "Failed to update order: {0}";
            public const string FailedToDelete = "Failed to delete order: {0}";
            public const string CancelNotAllowed = "Cannot cancel order: {0}";
        }

        public static class OrderDetail
        {
            // DTO validation messages
            public const string ProductIdRequired = "The product ID is required.";
            public const string QuantityRequired = "The quantity is required.";
            public const string QuantityRange = "The quantity for each product must be between 1 and 100.";

            // Service / repository messages
            public const string OrderDetailDataNull = "Order detail data cannot be null.";
            public const string NotFound = "Order detail with ID {0} not found.";
            public const string FailedToCreate = "Failed to create order detail: {0}";
            public const string FailedToUpdate = "Failed to update order detail: {0}";
            public const string FailedToDelete = "Failed to delete order detail: {0}";
        }

        public static class PaymentMethod
        {
            // DTO validation messages
            public const string PaymentTypeRequired = "The payment type is required.";
            public const string PaymentTypeTooLong = "The payment type cannot exceed 100 characters.";

            public const string ImageUrlRequired = "The image URL is required.";
            public const string ImageUrlTooLong = "The image URL cannot exceed 200 characters.";
            public const string ImageUrlInvalid = "The image URL must be a valid URL.";

            // Service / repository messages
            public const string PaymentMethodDataNull = "Payment method data cannot be null.";
            public const string NotFound = "Payment method with ID {0} not found.";
            public const string FailedToCreate = "Failed to create payment method: {0}";
            public const string FailedToUpdate = "Failed to update payment method: {0}";
            public const string FailedToDelete = "Failed to delete payment method: {0}";
        }

        public static class Product
        {
            // DTO validation messages
            public const string NameRequired = "Product name is required.";
            public const string NameTooLong = "Product name cannot be longer than 255 characters.";
            public const string EnglishNameTooLong = "English name cannot be longer than 255 characters.";
            public const string DescriptionRequired = "Description is required.";

            public const string PriceRequired = "Price is required.";
            public const string PricePositive = "Price must be a positive number.";

            public const string MarketPriceRequired = "Market price is required.";
            public const string MarketPricePositive = "Market price must be a positive number.";

            public const string QuantityInStockRequired = "Quantity in stock is required.";
            public const string QuantityInStockNonNegative = "Quantity in stock cannot be negative.";

            // Service / repository messages
            public const string ProductDataNull = "Product data cannot be null.";
            public const string NotFound = "Product with ID {0} not found.";
            public const string FailedToCreate = "Failed to create product: {0}";
            public const string FailedToUpdate = "Failed to update product: {0}";
            public const string FailedToDelete = "Failed to delete product: {0}";
            public const string NameAlreadyExists = "Product name '{0}' already exists.";
            public const string InUseByOrderDetails = "Cannot delete product. It is currently used in one or more order details.";
        }

        public static class ProductAssociation
        {
            // DTO validation messages
            public const string ProductIdRequired = "Product ID is required.";
            public const string VariationOptionIdRequired = "Variation Option ID is required.";
            public const string SkinConditionIdRequired = "Skin Condition ID is required.";
            public const string SkinTypeIdRequired = "Skin Type ID is required.";

            // Service / repository messages
            public const string ProductAssociationDataNull = "Product association data cannot be null.";
            public const string NotFound = "Product association with ID {0} not found.";
            public const string FailedToCreate = "Failed to create product association: {0}";
            public const string FailedToUpdate = "Failed to update product association: {0}";
            public const string FailedToDelete = "Failed to delete product association: {0}";
        }

        public static class ProductCategory
        {
            // DTO validation messages
            public const string CategoryNameRequired = "Category name is required.";
            public const string CategoryNameTooLong = "Category name cannot be longer than 100 characters.";
            public const string ParentCategoryIdInvalid = "Parent category ID is invalid."; // optional, dùng khi cần kiểm tra

            // Service / repository messages
            public const string ProductCategoryDataNull = "Product category data cannot be null.";
            public const string NotFound = "Product category with ID {0} not found.";
            public const string FailedToCreate = "Failed to create product category: {0}";
            public const string FailedToUpdate = "Failed to update product category: {0}";
            public const string FailedToDelete = "Failed to delete product category: {0}";
            public const string NameAlreadyExists = "Product category name '{0}' already exists.";
            public const string InUseByProducts = "Cannot delete category. It is currently used by one or more products.";
        }

        public static class ProductImage
        {
            // DTO validation messages
            public const string ImageUrlRequired = "The image URL is required.";
            public const string ImageUrlTooLong = "The image URL cannot exceed 500 characters.";
            public const string ImageUrlInvalid = "The image URL must be a valid URL.";
            public const string IsThumbnailRequired = "The thumbnail status is required."; // boolean required message

            // Service / repository messages
            public const string ProductImageDataNull = "Product image data cannot be null.";
            public const string NotFound = "Product image with ID {0} not found.";
            public const string FailedToCreate = "Failed to create product image: {0}";
            public const string FailedToUpdate = "Failed to update product image: {0}";
            public const string FailedToDelete = "Failed to delete product image: {0}";
            public const string ThumbnailConflict = "There is already a thumbnail for product {0}.";
        }

        public static class ProductStatus
        {
            // DTO validation messages
            public const string StatusNameRequired = "Status name is required.";
            public const string StatusNameTooLong = "Status name cannot be longer than 100 characters.";
            public const string DescriptionRequired = "Description is required.";
            public const string DescriptionTooLong = "Description cannot be longer than 500 characters.";

            // Service / repository messages
            public const string ProductStatusDataNull = "Product status data cannot be null.";
            public const string NotFound = "Product status with ID {0} not found.";
            public const string FailedToCreate = "Failed to create product status: {0}";
            public const string FailedToUpdate = "Failed to update product status: {0}";
            public const string FailedToDelete = "Failed to delete product status: {0}";
            public const string NameAlreadyExists = "Product status name '{0}' already exists.";
            public const string InUseByProducts = "Cannot delete product status. It is currently used by one or more products.";
        }

        public static class Variation
        {
            // DTO validation messages
            public const string NameRequired = "Variation name is required.";
            public const string NameTooLong = "Name cannot be longer than 255 characters.";
            public const string ProductCategoryIdRequired = "Product Category ID is required.";

            // Service / repository messages
            public const string VariationDataNull = "Variation data cannot be null.";
            public const string NotFound = "Variation with ID {0} not found.";
            public const string FailedToCreate = "Failed to create variation: {0}";
            public const string FailedToUpdate = "Failed to update variation: {0}";
            public const string FailedToDelete = "Failed to delete variation: {0}";
            public const string NameAlreadyExists = "Variation name '{0}' already exists.";
        }

        public static class VariationOption
        {
            // DTO validation messages
            public const string ValueRequired = "Option value is required.";
            public const string ValueTooLong = "Value cannot be longer than 255 characters.";
            public const string VariationIdRequired = "Variation ID is required.";

            // Service / repository messages
            public const string VariationOptionDataNull = "Variation option data cannot be null.";
            public const string NotFound = "Variation option with ID {0} not found.";
            public const string FailedToCreate = "Failed to create variation option: {0}";
            public const string FailedToUpdate = "Failed to update variation option: {0}";
            public const string FailedToDelete = "Failed to delete variation option: {0}";
            public const string ValueAlreadyExists = "Variation option value '{0}' already exists for variation '{1}'.";
        }

        public static class Voucher
        {
            // DTO validation messages
            public const string CodeRequired = "Voucher code is required.";
            public const string CodeTooLong = "Voucher code cannot be longer than 100 characters.";

            public const string DescriptionRequired = "Description is required.";
            public const string DescriptionTooLong = "Description cannot be longer than 500 characters.";

            public const string DiscountRateRequired = "Discount rate is required.";
            public const string DiscountRateOutOfRange = "Discount rate must be between 0 and 100.";

            public const string MinimumOrderValueRequired = "Minimum order value is required.";
            public const string MinimumOrderValuePositive = "Minimum order value must be a positive number.";

            public const string StartDateRequired = "Start date is required.";
            public const string EndDateRequired = "End date is required.";

            public const string UsageLimitRequired = "Usage limit is required.";
            public const string UsageLimitAtLeastOne = "Usage limit must be at least 1.";

            // Service / repository messages
            public const string VoucherDataNull = "Voucher data cannot be null.";
            public const string NotFound = "Voucher with ID {0} not found.";
            public const string FailedToCreate = "Failed to create voucher: {0}";
            public const string FailedToUpdate = "Failed to update voucher: {0}";
            public const string FailedToDelete = "Failed to delete voucher: {0}";
            public const string CodeAlreadyExists = "Voucher code '{0}' already exists.";
            public const string VoucherExpired = "Voucher '{0}' is expired.";
            public const string VoucherNotActive = "Voucher '{0}' is not active.";
        }

        public static class General
        {
            public const string UnexpectedError = "An unexpected error occurred: {0}";
        }
    }
}