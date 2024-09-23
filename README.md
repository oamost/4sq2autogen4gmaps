# Samples

## Utility usage:

![image](https://github.com/user-attachments/assets/8f5b25e4-4a2e-4eca-bade-2a5ecd605145)

## Generated output:

![image](https://github.com/user-attachments/assets/2107ef94-7a41-44be-a690-ba682fb00103)

## Google Maps use case:

![image](https://github.com/user-attachments/assets/5f9aea1b-7eee-4d4d-bbc4-c169d38df148)

# How-to use it?

You will need dotnet-sdk-bin for building the project.
You can build it with dotnet restore && dotnet run

1. Start the built 4sq2autogen4gmaps utility and open Authenticate.html from src in your browser
   You will paste the "access code" from step 1-7 here
   
2. Create new project after logged into fsq/developer interface at https://foursquare.com/developers/home

3. On the new project's settings page, find OAuth Authentication

4. into redirect URL type the following: https://www.example.com 
   then click on save

5. Copy client ID and paste it into Authenticate.html text field

6. Click on Connect

7. Foursquare asks if you want to connect this app to your profile, click Allow

8. You are redirected to example.com, now copy the token after example.com/?code=
   from the URL bar

9. Paste this into terminal as mentioned in step #0 as the terminal asks for "access code"
10. Paste your project's Client ID and paste it into the terminal when asked
11. Do the same with Client Secret

12. Wait a bit. The following should appear in 2 minutes: "KML generation completed..."

13. open https://www.google.com/maps/d/u/0/?hl=en and create your new map
14. click on "New Layer", then click on `Import` and browse the `.kml` file located in the repo
15. the rest is straightforward - you can set a few map styles and you can set the venue labels to be shown on Google My Maps
