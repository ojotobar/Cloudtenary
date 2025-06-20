# Cloudtenary Package

## Table of Contents
1. [Introduction](#introduction)
2. [Getting Started](#getting-start)
3. [Usage Guide](#usage-guide)
4. [Links](#links)

### Introduction
***
This package is aimed at simplifying and speeding up connection to Cloudinary. Provides utility methods for uploading to and deleting files from your Cloudinary account.

## Getting Started
***
Download the package ```Cloudtenary```. Add ```CloudtenarySettings``` section to your appsettings or secrets and specify CloudName, Key, and Secret keys and their values.
* In the ```Program.cs``` or the ```Startup.cs``` class, add the ```AddCloudtenary()``` method to the service pipeline. In addition to the ```IServiceCollections```, the extension method also accepts the ```IConfiguration``` interface.

## Usage Guide
***
After registering the package, you can then inject the ```ICloudtenary``` interface into your class to call the methods defined.

## Links
***
To view the source code or get in touch:
* [Github Repository Link](https://github.com/ojotobar/Cloudtenary)
* [Send Me A Mail](mailto:ojotobar@gmail.com)