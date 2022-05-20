![SEYR](https://user-images.githubusercontent.com/19335151/165121032-fe8c9a68-3cf7-4112-8ee5-a5f04922ef1c.png) ![Nuget](https://img.shields.io/nuget/v/SEYR)
# AOI for present / not-present analysis

### How to integrate SEYR into an existing .NET Framework project (Nuget v1.3.13+)
Create an instance of a SEYR channel

```cs
private static SEYR.Session.Channel SEYRCh = null;

private void openDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
{
    SEYR.Session.Channel channel = SEYR.Session.Channel.OpenSEYR();
    if (channel != null) 
    {
        SEYRCh = channel;
        SEYRCh.SetPixelsPerMicron(1.303f);
    }
}
```
Add some UI elements

```cs
private void openComposerToolStripMenuItem_Click(object sender, EventArgs e)
{
    SEYRCh.OpenComposer(LastImage);
}

private async void forcePatternToolStripMenuItem_Click(object sender, EventArgs e)
{
    await SEYRCh.NewImage(LastImage, true, "");
}

private async void reloadImageToolStripMenuItem_Click(object sender, EventArgs e)
{
    await SEYRCh.NewImage(LastImage, false, "");
}
```
Send images to SEYR

```cs
private async Task<bool> Run()
{
    if (SEYRCh == null) return false;
    SEYRCh.ResetAll();

    for (int i = 0; i < Points.Count; i++)
    {
        ///Do something
        double info = await SEYRCh.NewImage(LastImage, false, $"{i}\t{Points[i].X}\t{Points[i].Y}\t{Points[i].Info}");
        GC.Collect();
    }
    
    SEYRCh.MakeArchive(true);

    bool test = true;
    return test;
}
```
#### Notes
- Image processing must either be awaited or contain a while loop that waits until Channel.Working == false. 
- Custom filters can be created by building a local copy of SEYR. The v1.3.4 default custom feature is copied from [this repository](https://github.com/bradmartin333/GridImaging).
