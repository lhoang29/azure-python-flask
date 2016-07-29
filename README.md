# Python Flask on Azure
Skeleton project for deploying [Python Flask](http://flask.pocoo.org/) to Azure using [Python Tools For Visual Studio](https://github.com/Microsoft/PTVS). 

Prerequisites: Visual Studio 2013 with Python Tools for Visual Studio installed.

**Note**: The recommended way to deploy a Python Flask web service to Azure is outlined [here](https://azure.microsoft.com/en-us/documentation/articles/cloud-services-python-ptvs/#install-python-on-the-cloud-service). However, in case some Python packages are required that cannot be easily installed via pip or the default PTVS environment, this method should work for any arbitrary Python distribution. 

Follow these below steps to get your service running: 

1. Build the Python distribution on your local development machine. [Miniconda](http://conda.pydata.org/miniconda.html) should be useful to install the right packages without much efforts. Zip and upload the Python distribution to a publicly available location (e.g.: http://myhost.com/MyPythonFile.zip). For example, this could be your Miniconda installation folder.    
1. Clone this repository and open the solution in Visual Studio 2013. 
1. In [setup/program.cs](https://github.com/lhoang29/azure-python-flask/blob/master/setup/Program.cs), update the base address and name of your package (e.g. base address = http://myhost.com/ and package name = MyPythonFile.zip). 
1. Right click on "cloud" project and click Publish. For debugging purposes, you can optionally enable RemoteAccess.
1. If everything works, visit the published cloud service's address in a browser. You should see a message of the form: "Python Flask is up and running!".


