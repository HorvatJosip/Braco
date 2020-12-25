using Braco.Services.Abstractions;
using System;
using System.IO;

namespace Braco.Services
{
	/// <summary>
	/// Implementation of <see cref="IAuthService"/> using file as storage.
	/// </summary>
    public class FileAuthService : IAuthService
    {
        private const string separator = "_|_|_";

		/// <summary>
		/// Key used for the file.
		/// </summary>
        public const string FileKey = "AuthFile";
		/// <summary>
		/// Name of the file.
		/// </summary>
        public const string FileName = "Auth.dat";

        private readonly FileInfo _authFile;
        private readonly ISecurityService _securityService;

		/// <inheritdoc/>
        public string CurrentUser { get; private set; }

		/// <inheritdoc/>
        public bool IsAuthenticated => CurrentUser != null;

		/// <inheritdoc/>
        public string UnauthenticatedLocation { get; } = "Login";

		/// <inheritdoc/>
        public event EventHandler<AuthEventArgs> AuthActionOccurred;

		/// <summary>
		/// Creates an instance of the file auth service.
		/// </summary>
		/// <param name="securityService">Security service that is required
		/// for securing the data.</param>
		/// <param name="fileManager">Path manager used in the project.</param>
        public FileAuthService(ISecurityService securityService, IFileManager fileManager)
        {
            _securityService = securityService ?? throw new ArgumentNullException(nameof(securityService));

            _authFile = fileManager.AddFileToDirectory(FileKey, fileManager.AppDataDirectory, FileName);
        }

		/// <inheritdoc/>
        public bool LogIn(string identifier, string password)
        {
            using var reader = new StreamReader(_authFile.OpenRead());

            string line;

            while ((line = reader.ReadLine()) != null)
            {
                var parts = line.Split(separator);

                if (parts.Length == 2)
                {
                    if
                    (
                        _securityService.MatchesHash(identifier, parts[0]) &&
                        _securityService.MatchesHash(password, parts[1])
                    )
                    {
                        CurrentUser = identifier;
                        AuthActionOccurred?.Invoke(this, new AuthEventArgs(AuthAction.LogIn));
                        return true;
                    }
                }
            }

            return false;
        }

		/// <inheritdoc/>
        public bool LogOut()
        {
            CurrentUser = null;
            AuthActionOccurred?.Invoke(this, new AuthEventArgs(AuthAction.LogOut));
            return true;
        }

		/// <inheritdoc/>
        public bool Register(string identifier, string password)
        {
            using (var reader = new StreamReader(_authFile.OpenRead()))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(separator);

                    if (parts.Length == 2)
                    {
                        if (_securityService.MatchesHash(identifier, parts[0]))
                            return false;
                    }
                }
            }

            using (var writer = new StreamWriter(_authFile.Open(FileMode.Append, FileAccess.Write)))
            {
                writer.WriteLine(string.Join
                (
                    separator,
                    _securityService.Hash(identifier),
                    _securityService.Hash(password)
                ));
            }

            CurrentUser = identifier;
            AuthActionOccurred?.Invoke(this, new AuthEventArgs(AuthAction.Register));

            return true;
        }
    }
}
