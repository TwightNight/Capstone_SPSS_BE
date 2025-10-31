namespace SPSS.Shared.Constants
{
    public static class ExceptionMessageConstants
    {
        public static class Brand
        {
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
            public const string CountryDataNull = "Country data cannot be null.";
            public const string NotFound = "Country with ID {0} not found.";
            public const string CodeAlreadyExists = "Country code '{0}' already exists.";
            public const string NameAlreadyExists = "Country name '{0}' already exists.";
            public const string InUseByAddresses = "Cannot delete country. It is currently in use by one or more addresses.";
        }


        public static class SkinCondition
        {
            public const string SkinConditionDataNull = "Skin condition data cannot be null.";
            public const string NotFound = "Skin condition with ID {0} not found.";
            public const string FailedToSave = "Failed to save skin condition: {0}";
            public const string FailedToDelete = "Failed to delete skin condition: {0}";
            public const string NameAlreadyExists = "Skin condition name '{0}' already exists.";
            public const string InUseByUser = "Cannot delete skin condition. It is currently in use.";
        }

        public static class SkinType
        {
            public const string SkinTypeDataNull = "SkinType data cannot be null.";
            public const string NotFound = "SkinType with ID {0} not found.";
            public const string FailedToCreate = "Failed to create skin type: {0}";
            public const string FailedToUpdate = "Failed to update skin type: {0}";
            public const string FailedToDelete = "Failed to delete skin type: {0}";
            public const string NameAlreadyExists = "Skin type name '{0}' already exists.";
            public const string InUseByProduct = "Cannot delete skin type. It is currently in use by one or more products.";
        }

        public static class General
        {
            public const string UnexpectedError = "An unexpected error occurred: {0}";
        }
    }
}