# Screenshots

## Utility usage:

![image](https://github.com/user-attachments/assets/8f5b25e4-4a2e-4eca-bade-2a5ecd605145)

## Generated output:

![image](https://github.com/user-attachments/assets/2107ef94-7a41-44be-a690-ba682fb00103)

## Google Maps use case:

![image](https://github.com/user-attachments/assets/5f9aea1b-7eee-4d4d-bbc4-c169d38df148)

# How do I use it?

1. Download `authenticate.zip` and `yourplatform_x64.zip` from the releases page.
   Unzip them, and launch `Authenticate.html` in the browser, thus `./4sq2autogen4gmaps` (.exe, or .out executable) in your terminal.
  
3. In your browser, go to `https://foursquare.com/developers/home` to create a new project after logging into the interface.

4. On the new project's settings page, find OAuth Authentication.

5. Find the redirect URL entry, and type the following into it: https://www.example.com 
   then click on save.

6. Copy the client ID and paste it into the Authenticate.html text field.

7. Click on Connect. Foursquare asks if you want to connect this app to your profile, click Allow.

8. You are redirected to example.com, now copy the token after example.com/?code=
   from the URL bar. Only the part after the equality sign.
   
9. Paste this into your terminal where you have the launched ./4sq2autogen4gmaps waiting for the `access code`.

10. Paste your project's Client ID and paste it into the terminal when asked.

11. Do the same with Client Secret.

12. Wait a bit. The following should appear in 2 minutes: "KML generation completed..."

13. Open https://www.google.com/maps/d/u/0/?hl=en and create your new map.

14. click on "New Layer", then click on `Import` and browse the `.kml` file located in the repo.

15. the rest is straightforward - you can set a few map styles and you can set the venue labels to be shown on Google My Maps.
