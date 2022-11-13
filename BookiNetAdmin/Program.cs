// See https://aka.ms/new-console-template for more information
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;

internal class Program
{
	private static async Task Create(string email, string password)
	{
		UserRecordArgs uArgs = new()
		{
			Email = email,
			Password = password
		};
		UserRecord user = await FirebaseAuth.DefaultInstance.CreateUserAsync(uArgs);
		await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(
			user.Uid, new Dictionary<string, object>() { { "admin", true } });
	}

	private static async Task Modify(string email, bool demote)
	{
		UserRecord user = await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(email);
		await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(
			user.Uid, demote ? 
				new Dictionary<string, object>() 
				: new Dictionary<string, object>() { { "admin", true } });

	}

	private static async Task<int> Main(string[] args)
	{
		FirebaseApp.Create(new AppOptions()
		{
			Credential = GoogleCredential.FromFile("./boookie-28f8d-firebase-adminsdk-8k7t3-913cf19704.json"),
		});

		switch (args[0]) {
			case "--c":
				await Create(args[1], args[2]);
				break;
			case "--p":
				await Modify(args[1], false);
				break;
			case "--d":
				await Modify(args[1], true);
				break;
			default:
				throw new ArgumentException("Invalid option");
		};

		return 0;
	}
}