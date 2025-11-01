namespace SPSS.Shared.Constants
{
	/// <summary>
	/// Centralized exception/error message constants grouped by domain (Address, User, Country, Transaction, Authentication, Token, Blog, BlogSection, ChatHistory, Reply, Role, SkinCondition, SkinType, Validation, General).
	/// Use string.Format or interpolation when inserting runtime values.
	/// </summary>
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
            public const string NotFoundByEmail = "User not found with the specified email.";
            public const string UserDeleted = "User with ID {0} is deleted.";
            public const string EmailTaken = "Email '{0}' is already in use.";
            public const string UsernameTaken = "Username '{0}' is already in use.";
            public const string PhoneTaken = "Phone number '{0}' is already in use.";
            public const string AvatarUrlInvalid = "Avatar URL cannot be null or empty.";
            public const string AccountNotFound = "Account for user {0} not found.";
            public const string UserDataNull = "User data cannot be null.";
            public const string NotFoundByUsername = "User with username '{0}' not found.";
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
            #region General
            public const string InvalidArgument = "Invalid argument: {0}";
            public const string MissingField = "Missing required field: {0}";
            #endregion

            #region Field-specific 'Required' messages
            // Account & User
            public const string UserNameIsRequired = "User name is required.";
            public const string UsernameIsRequiredForRegister = "Username is required.";
            public const string UsernameOrEmailIsRequired = "Username or Email is required";
            public const string SurNameIsRequired = "Surname is required.";
            public const string LastNameIsRequired = "Last name is required.";
            public const string EmailIsRequired = "Email address is required.";
            public const string PhoneNumberIsRequired = "Phone number is required.";
            public const string AvatarUrlIsRequired = "Avatar URL is required.";
            public const string PasswordIsRequired = "Password is required";
            public const string CurrentPasswordIsRequired = "Current password is required.";
            public const string NewPasswordIsRequired = "New password is required.";
            public const string StatusIsRequired = "Status is required.";

            // Address
            public const string CountryIdIsRequired = "CountryId is required.";
            public const string CustomerNameIsRequired = "Customer name is required.";
            public const string StreetNumberIsRequired = "Street number is required.";
            public const string AddressLine1IsRequired = "AddressLine1 is required.";
            public const string CityIsRequired = "City is required.";
            public const string WardIsRequired = "Ward is required.";
            public const string PostcodeIsRequired = "Postcode is required.";
            public const string ProvinceIsRequired = "Province is required.";

            // Authentication & Role
            public const string UserIdIsRequired = "User ID is required.";
            public const string RoleIdIsRequired = "Role ID is required.";
            public const string RoleNameIsRequired = "Role name is required";
            public const string RefreshTokenIsRequired = "Refresh token is required.";

            // Blog & BlogSection
            public const string TitleIsRequired = "Title is required.";
            public const string DescriptionIsRequired = "Description is required.";
            public const string ThumbnailUrlIsRequired = "Thumbnail URL is required.";
            public const string BlogSectionsAreRequired = "Blog sections are required.";
            public const string ContentTypeIsRequired = "Content type is required.";
            public const string SubtitleIsRequired = "Subtitle is required.";
            public const string ContentIsRequired = "Content is required.";

            // Country
            public const string CountryCodeIsRequired = "Country code is required.";
            public const string CountryNameIsRequired = "Country name is required.";

            // ChatHistory
            public const string MessageContentIsRequired = "Message content is required.";
            public const string SenderTypeIsRequired = "Sender type is required.";
            public const string SessionIdIsRequired = "Session ID is required.";

            // Reply & Review
            public const string ReviewIdIsRequired = "Review ID is required.";
            public const string ReplyContentIsRequired = "Reply content is required.";
            public const string ProductItemIdIsRequired = "Product item ID is required.";
            public const string RatingValueIsRequired = "Rating value is required.";

            // SkinType & SkinCondition
            public const string SkinTypeNameIsRequired = "Skin type name is required.";
            public const string SkinConditionNameIsRequired = "Name is required.";

            // Transaction
            public const string TransactionIdIsRequired = "Transaction ID is required.";
            public const string TransactionTypeIsRequired = "Transaction type is required.";
            public const string AmountIsRequired = "Amount is required.";

            // OTP
            public const string OtpCodeIsRequired = "OTP code is required.";
            #endregion

            #region Format validation
            public const string InvalidEmailFormat = "Invalid email address format.";
            public const string InvalidPhoneFormat = "Invalid phone number format.";
            public const string InvalidUrlFormat = "Invalid URL format.";
            public const string InvalidThumbnailUrl = "The Thumbnail must be a valid URL.";
            public const string InvalidRoleForPrivilegedRegister = "Role must be either 'Manager' or 'Staff'.";
            public const string InvalidSenderType = "Sender type must be either 'User' or 'Bot'.";
            public const string InvalidTransactionType = "Transaction type must be either 'Deposit' or 'Withdrawal'.";
            public const string InvalidTransactionStatus = "Status must be 'Success', 'Failed', or 'Pending'.";
            #endregion

            #region Length & Range validation
            // Generic
            public const string DescriptionTooLong500 = "Description cannot exceed 500 characters";
            public const string DescriptionTooLong1000 = "Description cannot exceed 1000 characters.";

            // Account & User
            public const string UsernameMinLength = "Username must be at least 3 characters.";
            public const string PasswordMinLength = "Password must be at least 8 characters.";
            public const string NewPasswordMinLength = "The new password must be at least 8 characters long.";
            public const string SurNameTooLong = "Surname cannot exceed 100 characters.";
            public const string LastNameTooLong = "Last name cannot exceed 100 characters.";
            public const string EmailTooLong = "Email address cannot exceed 100 characters.";
            public const string PhoneNumberTooLong = "Phone number cannot exceed 20 characters.";
            public const string AvatarUrlTooLong = "Avatar URL cannot exceed 500 characters.";
            public const string DietTooLong = "Diet description cannot exceed 1000 characters.";
            public const string DailyRoutineTooLong = "Daily routine description cannot exceed 1000 characters.";
            public const string AllergyTooLong = "Allergy information cannot exceed 1000 characters.";
            public const string CertificateTooLong = "Certificate information cannot exceed 1000 characters.";
            public const string InvalidAgeRange = "Age must be between 1 and 150.";

            // Address
            public const string CustomerNameTooLong = "Customer name cannot exceed 200 characters.";
            public const string StreetNumberTooLong = "Street number cannot exceed 50 characters.";
            public const string AddressLine1TooLong = "AddressLine1 cannot exceed 200 characters.";
            public const string AddressLine2TooLong = "AddressLine2 cannot exceed 200 characters.";
            public const string CityTooLong = "City cannot exceed 100 characters.";
            public const string WardTooLong = "Ward cannot exceed 100 characters.";
            public const string PostcodeTooLong = "Postcode cannot exceed 20 characters.";
            public const string ProvinceTooLong = "Province cannot exceed 100 characters.";

            // Blog & BlogSection
            public const string TitleTooLong = "Title cannot exceed 200 characters.";
            public const string ThumbnailUrlTooLong = "Thumbnail URL cannot exceed 500 characters.";
            public const string AtLeastOneSectionIsRequired = "At least one section is required.";
            public const string ContentTypeTooLong = "Content type cannot exceed 50 characters.";
            public const string SubtitleTooLong = "Subtitle cannot exceed 200 characters.";
            public const string OrderMustBeNonNegative = "Order must be a non-negative number.";

            // Country
            public const string CountryCodeTooLong = "Country code cannot exceed 10 characters.";
            public const string CountryNameTooLong = "Country name cannot exceed 100 characters.";

            // ChatHistory
            public const string MessageContentTooLong = "Message content cannot exceed 4000 characters.";
            public const string SessionIdTooLong = "Session ID cannot exceed 100 characters.";

            // Reply & Review
            public const string ReplyContentTooLong = "Reply content cannot exceed 1000 characters.";
            public const string CommentTooLong = "Comment cannot exceed 1000 characters.";
            public const string InvalidRatingRange = "Rating value must be between 0 and 5.";

            // Role
            public const string RoleNameTooLong = "Role name cannot exceed 100 characters";

            // SkinCondition & SkinType
            public const string SkinConditionNameTooLong = "Name cannot exceed 200 characters.";
            public const string InvalidSeverityLevelRange = "Severity level must be between 1 and 10.";
            public const string SkinTypeNameTooLong = "Skin type name cannot exceed 100 characters.";
            public const string RoutineDetailsTooLong = "Routine details cannot exceed 2000 characters.";

            // Transaction
            public const string AmountMustBeAtLeast1000 = "Amount must be at least 1000.";
            #endregion
        }


        public static class Authentication
        {
            public const string InvalidCredentials = "Invalid username/email or password.";
            public const string CurrentPasswordIncorrect = "Current password is incorrect.";
            public const string InvalidPasswordFormat = "Password is not valid. Must be at least 8 characters, include uppercase, lowercase, number, and special character.";
            public const string AccountNotActive = "Account has not been activated. Please check your email to verify your account.";

            public const string UsernameTaken = "Username '{0}' is already taken.";
            public const string EmailTaken = "Email '{0}' is already taken.";
            public const string RegistrationFailed = "Registration failed: {0}";
            public const string RegistrationFailedGeneric = "Registration failed due to an unexpected error."; // ADDED
            public const string AccountAlreadyActive = "Account is already activated."; // ADDED
            public const string RefreshTokenNotFound = "Refresh token not found.";
            public const string RefreshTokenRevoked = "Refresh token has been revoked.";
        }

        public static class Otp
        {
            public const string KeyNotConfigured = "OTP key not configured.";
        }

        // ADDED NEW CLASS
        public static class Verification
        {
            public const string NotFound = "Verification record not found.";
            public const string CodeNoLongerValid = "This verification code is no longer valid.";
            public const string CodeExpired = "Verification code has expired.";
            public const string InvalidCode = "Invalid verification code.";
            public const string ResendCooldown = "Please wait before requesting another code. Try again in {0} seconds.";
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
            public const string NotOwner = "You are not the owner of this reply.";

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