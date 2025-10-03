using System;
using System.IO;
using System.Threading.Tasks;

namespace RedsMapCommands;

using Google.Cloud.Firestore;

public static class WaypointStore
{
    private static FirestoreDb firestoreDb;
    private static string configFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"VintagestoryData\ModConfig\RedsMapCommands\");
    private static void InitFirestore()
    {
        try
        {
            if (string.IsNullOrEmpty( Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS"))){
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
            }
            
            // Initialize FirestoreDb. The project ID is read from the credentials file.
            if (firestoreDb==null) 
                firestoreDb = FirestoreDb.Create("ilu-ambar-ce3ed"); // Pass your Firebase Project ID here if needed, e.g., FirestoreDb.Create("my-project-id")
            
            Console.WriteLine("[MyFirebaseMod] Successfully connected to Firestore.");
        }
        catch (Exception e)
        {
            Console.WriteLine("[MyFirebaseMod] Failed to initialize Firestore: {0}", e.ToString());
        }
    }
    
    public static void WriteToMap(string collectionName,string layer,object markerData, string[] coords = null)
    {
        // Only initializes Firestore if the command runs.
        InitFirestore();
        DocumentReference docRef = null;
        // Firestore operations are async and should not block the main game thread!
        // We run it as a background task to avoid freezing the server.
        Task.Run(async () =>
        {
            try
            {
                docRef = firestoreDb.Collection(collectionName).Document(layer);
                
                // SetAsync will create the document if it doesn't exist or overwrite it if it does.
                await docRef.SetAsync(markerData, SetOptions.MergeAll);
                if(coords!=null)
                    await docRef.UpdateAsync("Coords", FieldValue.ArrayUnion(coords));
                Console.WriteLine("[MyFirebaseMod] Wrote player data to Firestore.");
            }
            catch (Exception e)
            {
                Console.WriteLine("[MyFirebaseMod] Error writing to Firestore: {0}", e.Message);
            }
        });
    }

    public static void WriteWaypoint(string collectionName, string layer, string player, object markerData)
    {
        InitFirestore();
        DocumentReference docRef = null;
        try
        {
            Task.Run(async () =>
            {
              
            });
        }
        catch (Exception e)
        {
            Console.WriteLine("[MyFirebaseMod] Error writing to Firestore: {0}", e.Message);
        }
    }

    private static void WriteDocRefFor(string player, string document, DocumentReference docRef)
    {
        
    }

    public static string UpdateMapControlCoords(string coords)
    {
        InitFirestore();
        Task.Run(async () =>
        {
            var collection_name = "configuration";
            var data = new
            {
                spawnControl=coords,
                lastUpdateTime=DateTime.Now
            };
            try
            {
                var result = await firestoreDb.Collection(collection_name)
                    .Document("map")
                    .SetAsync(data, SetOptions.MergeAll);
            }
            catch (Exception e)
            {
                Console.WriteLine("[MyFirebaseMod] Error writing to Firestore: {0}", e.ToString());
            }

            return $"Spawn Control variable updated {coords}.";
        });
        return string.Empty;
    }
    
}