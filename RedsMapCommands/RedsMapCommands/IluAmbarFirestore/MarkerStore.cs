namespace IluAmbarFirestore;

using Google.Cloud.Firestore;

public class MarkerStore
{
    private FirestoreDb firestoreDb;
    
    public MarkerStore()
    {
        var configFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"VintagestoryData\ModConfig\RedsMapCommands\");
        try
        {
            // Path to your credentials file. The server admin must place this file here.
            // Using "ModConfig" is a good practice.
            string credentialsPath = Path.Combine(configFolder, "myfirebasemod-credentials.json");

            if (!File.Exists(credentialsPath))
            {
                Console.WriteLine("[MyFirebaseMod] Firebase credentials file not found at: {0}", credentialsPath);
                return;
            }

            // Set the environment variable programmatically for the current process.
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialsPath);

            // Initialize FirestoreDb. The project ID is read from the credentials file.
            firestoreDb = FirestoreDb.Create("ilu-ambar-ce3ed"); // Pass your Firebase Project ID here if needed, e.g., FirestoreDb.Create("my-project-id")
            Console.WriteLine("[MyFirebaseMod] Successfully connected to Firestore.");
        }
        catch (Exception e)
        {
            Console.WriteLine("[MyFirebaseMod] Failed to initialize Firestore: {0}", e.ToString());
        }
    }

    public void WriteToDb()
    {
        var playerUID ="";
        var PlayerName = "";
        var position = "";
        var layer = "";
        // Firestore operations are async and should not block the main game thread!
        // We run it as a background task to avoid freezing the server.
        Task.Run(async () =>
        {
            try
            {
                string playerUid = playerUID;
                DocumentReference docRef = firestoreDb.Collection("players").Document(playerUid);

                var markerData = new
                {
                    PlayerName = PlayerName,
                    DateAdded = Timestamp.GetCurrentTimestamp(),
                    FeatureGroup=layer,
                    Coords=position, //FIXME: Needs to be an aray
                };

                // SetAsync will create the document if it doesn't exist or overwrite it if it does.
                await docRef.SetAsync(markerData, SetOptions.MergeAll);

                Console.WriteLine("[MyFirebaseMod] Wrote player data for {0} to Firestore.", PlayerName);
            }
            catch (Exception e)
            {
                Console.WriteLine("[MyFirebaseMod] Error writing to Firestore for player {0}: {1}", PlayerName,
                    e.Message);
            }
        });
    }
    
    
}