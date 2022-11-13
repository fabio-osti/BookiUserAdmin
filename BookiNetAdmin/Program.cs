// See https://aka.ms/new-console-template for more information
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;

FirebaseApp.Create(new AppOptions()
{
	Credential = GoogleCredential.FromFile("./boookie-28f8d-firebase-adminsdk-8k7t3-913cf19704.json"),
});

UserRecord user = await FirebaseAuth.DefaultInstance
	.GetUserByEmailAsync(args[0]);
// Confirm user is verified.
if (user.EmailVerified) {
	var isNotAdmin = !user.CustomClaims.Keys.Contains("admin");
	var claims = isNotAdmin ? new Dictionary<string, object>()
	{
		{ "admin", true },
	} : new Dictionary<string, object>();
	await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(user.Uid, claims);
	Console.WriteLine($"User {(isNotAdmin ? "promoted" : "demoted")}.");
}