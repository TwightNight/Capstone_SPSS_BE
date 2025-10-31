namespace SPSS.Shared.Constants
{
	public static class ExceptionMessageConstants
	{
		public static class Address
		{
			// Validation
			public const string AddressDataNull = "Address data cannot be null.";

			// Not found
			public const string NotFound = "Address with ID {0} not found.";
			public const string NotFoundForUser = "Address with ID {0} not found for the current user.";

			// Business rules
			public const string CannotDeleteDefault = "Cannot delete the default address. Set another address as default first.";

			// Transactional
			public const string FailedToCreate = "Failed to create address: {0}";
			public const string FailedToSetAsDefault = "Failed to set address as default: {0}";
		}

		public static class Country
		{
			public const string CountryDataNull = "Country data cannot be null.";
			public const string NotFound = "Country with ID {0} not found.";
			public const string CodeAlreadyExists = "Country code '{0}' already exists.";
			public const string NameAlreadyExists = "Country name '{0}' already exists.";
			public const string InUseByAddresses = "Cannot delete country. It is currently in use by one or more addresses.";
		}

		public static class User
		{
			public const string NotFound = "User with ID {0} not found.";
			public const string UserDeleted = "User with ID {0} is deleted.";
			public const string EmailTaken = "Email '{0}' is already in use.";
			public const string UsernameTaken = "Username '{0}' is already in use.";
			public const string PhoneTaken = "Phone number '{0}' is already in use.";
			public const string AvatarUrlInvalid = "Avatar URL cannot be null or empty.";
			public const string AccountNotFound = "Account for user {0} not found.";
		}

		public static class Transaction
		{
			public const string NotFound = "Transaction with ID {0} not found.";
			public const string UserNotFound = "User with ID {0} not found.";
			public const string FailedToCreate = "Failed to create transaction: {0}";
			public const string FailedToGenerateQr = "Failed to generate QR code for payment: {0}";
			public const string FailedToGet = "Failed to get transaction: {0}";
			public const string FailedToUpdateStatus = "Failed to update transaction status: {0}";
		}

		public static class Token
		{
			public const string JwtKeyNotConfigured = "JWT Key is not configured in appsettings.";
			public const string InvalidRefreshToken = "Invalid, used, revoked, or expired refresh token.";
			public const string InvalidRefreshTokenSimple = "Invalid refresh token.";
			public const string UserNotFound = "User not found.";
			public const string InvalidAccessToken = "Invalid access token.";
		}

		public static class Validation
		{
			public const string InvalidArgument = "Invalid argument: {0}";
			public const string MissingField = "Missing required field: {0}";
		}

		public static class Authentication
		{
			public const string InvalidCredentials = "Invalid username/email or password.";
			public const string CurrentPasswordIncorrect = "Current password is incorrect.";
			public const string InvalidPasswordFormat = "Password is not valid. Must be at least 8 characters, include uppercase, lowercase, number, and special character.";
			public const string UsernameTaken = "Username '{0}' is already taken.";
			public const string EmailTaken = "Email '{0}' is already taken.";
			public const string RegistrationFailed = "Registration failed: {0}";
			public const string RefreshTokenNotFound = "Refresh token not found.";
			public const string RefreshTokenRevoked = "Refresh token has been revoked.";
		}

		public static class Blog
		{
			// Validation
			public const string BlogDataNull = "Blog data cannot be null.";

			// Not found
			public const string NotFound = "Blog with ID {0} not found.";

            // Business rules (Security)
            public const string NotOwner = "You are not the owner of this blog.";

            // Operation failures
            public const string FailedToCreate = "Failed to create blog: {0}";
			public const string FailedToUpdate = "Failed to update blog: {0}";
			public const string FailedToDelete = "Failed to delete blog: {0}";
		}

		public static class BlogSection
		{
			public const string NotFound = "Blog section with ID {0} not found.";
			public const string SyncFailed = "Failed to synchronize blog sections: {0}";
		}

		public static class ChatHistory
		{
			// Validation
			public const string ChatHistoryDataNull = "Chat history data cannot be null.";

			// Not found
			public const string NotFoundForSession = "Chat session '{0}' not found.";
			public const string NotFoundForUserSession = "No chat history found for user {0} and session '{1}'.";
			public const string RecentSessionsNotFound = "No recent sessions found for user {0}.";

			// Operation failures
			public const string FailedToSave = "Failed to save chat message: {0}";
		}

		public static class Reply
		{
			public const string ReplyDataNull = "Reply data cannot be null.";
			public const string ReplyNotFound = "Reply with ID {0} not found.";
			public const string ReviewNotFound = "The specified reviewId does not exist.";
			public const string FailedToSave = "Failed to save reply: {0}";
		}

		public static class Role
		{
			public const string RoleDataNull = "Role data cannot be null.";
			public const string NotFound = "Role with ID {0} not found.";
			public const string NotFoundByName = "Role '{0}' does not exist.";
            public const string RoleNameAlreadyExists = "Role name '{0}' already exists.";
            public const string InUseByUsers = "Cannot delete role. It is currently in use by one or more users.";
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