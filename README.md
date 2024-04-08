# See-through

This project is based on the concept of Qian Long's HololensARToolkit, and it has been developed into a more efficient X-ray effects. We propose a lightweight system that separates computationally intensive image recognition tasks from the HMD, achieving a "cloud computing" effect and reducing resource consumption.

# Deployment

Simply clone the project and add it to Unity Hub. If you encounter the MRTK selection interface when launching the application, please select the recommended plugin.

In the Asset->Scene folder, locate "SimpleScene". Double-click to open it, and select the "NetworkManager" inside. Modify the "Network Address" property to the IP address of the remote PC.

Modify line 241 in the "TestServer.cs" file to update the IP address in the TcpClient to your PC's address that connected to  ECM camera (for some reason, assigning the address to a variable and passing it into TcpClient does not trigger the TCP connection).

To deploy on Hololens, first ceck if there are any warnings in Edit->XR Plug-in Management. If there are warnings, please click on "Fix All" to resolve them. Next, go to File->Build Settings and select the UWP Platform. Change the Architecture to ARM64-bit. In the "Build and Run" options, choose "Remote Device" and select "Release" for the Build Configuration.

For more information about deploying on Hololens, please refer to the official website: https://learn.microsoft.com/en-us/windows/mixed-reality/develop/advanced-concepts/using-visual-studio?tabs=hl2.

# Testing

Make sure your remote PC is connected to any USB external camera. Select the MixedReality Playspace->ARCamera component in the Hierachy and choose "Open Vuforia Engine configuration" in the inspector for the Vuforia Behaviour script. Then, in the Camera Device property, locate your external camera.

When a See-through application starts on HoloLens, it automatically connects to the remote PC server. Therefore, please ensure that the Unity Editor on the remote PC is running and that the "Server Only" option in the top left corner is selected.

After aligning the camera with the QR code, you should be able to see the overlay of virtual objects in the Game panel of the Unity Editor on the remote PC. 

On the Hololens client, you need to first align the Hololens camera with the QR code for one-time calibration. Then, click the "Anchor" button in the field of view. When the white square disappears, it means that the Vuforia engine has been disabled.

QR code: Find hologram_barcode in Assets->QRCodeStuff. Print it to the appropriate size (I printed it at 6*6 cm). You can also modify the QR code and upload it to the Vuforia cloud. For specific instructions, refer to: https://developer.vuforia.com/library/objects/image-targets


# ECM-camera Connection

Place the file "TcpServer_gzip.py" from the "camera_server" folder on the PC that is connecting to the ECM camera. Run the script server and click on the "Connect" in the field of view in you Hololens. If everything is working correctly, you should be able to see the video image from the ECM camera at the bottom of the "pyramid" virtual object.



