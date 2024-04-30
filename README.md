Wacom Safe File Uploader

1. Pull project start in debug mode
2. Use swagger api ui and use "Sign in" first request content does not matter as there is no registration requirement therefore program will create new user and return access token containing newly created user id.
3. Copy access token and paste it in swagger authorization window by adding "Bearer " infront of it - "Bearer <access_token>" and proceed to upload file
4. After uploading file you can access it while using the same token I have added 2 EP's by name and the first uploaded file if it is only one.
    If you use the get by name EP insert the whole file name plus extension like that "icon.png"
5. As the focus of the task in not on login only save file handling and authentication there is no actual login and password hashing/encoding the system will generate new user save it db and return jwt
   containing user id which will be used to validate the token on authentication.
6. User should be able to only download files they uploaded. This is achieved by taking the uploaded file info from the Files table by user foreign key then looking up for the actual file in local directory.
7. Uploading files is straightforward as well program will validate the file (size, type) and save it locally and add file info to the Files table. 
