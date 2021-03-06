# Postcard App

A SPA application that takes image input from the user (e.g. drag and drop, file upload, camera, etc.), modifies the image, and sends it as an email to a specified recipient.

### Prerequisites

These are the prerequisite web technologies and database utilized in this application:
* Visual Studio Code (or Visual Studio 2017/2019) 
* ASP.NET Core 2.x
* AngularJS 1.7.x
* SQLite

## Features

* Captures image data from the user
  * File Upload By Browse Popup
  * File Upload By Drag & Drop
  * Take snapshot from webcam
* Modifies the image data to contain a message
* Sends an email containing the modified image as an attachment
* Graceful error handling
* History of previously sent images
* Geotag images

### To Do

* Full unit test procedures
* Capture multiple images and create a gif

## Deployment

Working on it...

## Built With

* [jQuery](https://jquery.com/) - Used for document traversal, manipulation & event handling
* [AngularJS](https://angularjs.org/) - The web framework used
* [Bootstrap](https://getbootstrap.com/) - For Responsive UI
* [RequireJS](https://requirejs.org/) - Managing scripts and module dependencies
* [FabricJS](http://fabricjs.com/) - Canvas library used for image modification
* [ng-file-upload](https://github.com/danialfarid/ng-file-upload) - Uploading image to server
* [ng-table](https://github.com/esvit/ng-table) - Table view of image listing
* [webcam](https://github.com/jonashartmann/webcam-directive) - Take snapshot from webcam
* [ipstack](https://ipstack.com/) - Get coordinates for geotagging

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE) file for details

## Acknowledgments

* Stack Overflow - who helps me for resolving some issues
* Google - thank you to be google.
