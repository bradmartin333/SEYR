![SEYR](https://user-images.githubusercontent.com/19335151/165121032-fe8c9a68-3cf7-4112-8ee5-a5f04922ef1c.png) ![Nuget](https://img.shields.io/nuget/v/SEYR)
#### AOI for present / not-present analysis of live or saved images.
#### SEYR.dll can be added to an existing .NET project for framegrabbers.
#### SEYRDesktop can be run for existing images.

## How to integrate SEYR.dll into an exiting .NET Framework project (Nuget v1.3.4+)
### Channels
- SEYR's "Session" namespace houses the Channel class. 
- If a [Channel is constructued with only a directory path](https://github.com/bradmartin333/SEYR/blob/637b27327cb87c60b721c60accababeb3caa84ab/SEYRproject/SEYRDesktop/FormMain.cs#L106), it will open an existing .seyr project.
- If a [Channel is constructed with a directory path and a pixels per micron value](https://github.com/bradmartin333/SEYR/blob/637b27327cb87c60b721c60accababeb3caa84ab/SEYRproject/SEYRDesktop/FormMain.cs#L108), a new .seyr project will be created at that path. Pixels per micron is a value that scales the actual pixels in SEYR images to match the coordinate space of the host program. This value is then scaled by the user in the SEYR composer.
- The "dataHeader" parameter in the Channel constructor contains the tab delimeted header for the data that will be provided with each image by the host program. The default header is: "ImageNumber\tX\tY\tRR\tRC\tR\tC\tSR\tSC\t".
### Loading Images
- Image processing must either be awaited or contain a while loop that waits until Channel.Working == false. The reason for this is that the overhead for a .NET TaskFactory actually makes SEYR run slower than just processing images one at a time.
- [Loading a new image](https://github.com/bradmartin333/SEYR/blob/53bd15335e012986d650a825d8d99a340c1b5b7d/SEYRproject/SEYRDesktop/FormMain.cs#L33) returns a string of the percent passing features in the image.
  - The bitmap passed into SEYR will be cloned and processed.
  - Force pattern can be a useful tool to link to a special button or checkbox so the operator can validate their pattern without opening the composer.
  - The imageInfo matches the dataHeader schema. There isn't much error prevention here, but it is easy to stick to the rules.
- Depending on the type of application, a [manual garbage collection](https://github.com/bradmartin333/SEYR/blob/53bd15335e012986d650a825d8d99a340c1b5b7d/SEYRproject/SEYRDesktop/FormMain.cs#L36) might be required.
- [Custom filters](https://github.com/bradmartin333/SEYR/blob/71c895f670465773c0bc00a33e932b7a4292b2cf/SEYRproject/SEYRDesktop/FormMain.cs#L170) can also be created by building a local copy of SEYR. Certain features might become built-in to SEYR and selectable in the composer. This is where open source contributions are welcome. The v1.3.4 default custom feature is copied from [this repository](https://github.com/bradmartin333/GridImaging).
### UI Elements
- One necessary button is [Open Composer](https://github.com/bradmartin333/SEYR/blob/53bd15335e012986d650a825d8d99a340c1b5b7d/SEYRproject/SEYRDesktop/FormMain.cs#L41), which requires a bitmap to be initialized.
- Other useful buttons are Force Pattern, Reload Image, Load New Image, and some sort of [Restart](https://github.com/bradmartin333/SEYR/blob/53bd15335e012986d650a825d8d99a340c1b5b7d/SEYRproject/SEYRDesktop/FormMain.cs#L44).
