# 🎧 SFXManager for Unity

*Used in our game [Under the Disco Lights – Wishlist now on Steam](https://store.steampowered.com/app/3700950/Under_the_Disco_Lights)*

A modular and expandable **Sound FX Manager** script designed for Unity 2021+ using **AudioMixerGroups** and optional **audio pooling** for optimized runtime performance.

## ✨ Features
- ✅ Pooling support to reuse AudioSources
- ✅ Random pitch support for more natural variation
- ✅ 3D or 2D spatial blend toggle
- ✅ Single clip or random clip array playback
- ✅ Works with Audio Mixer routing

## 📂 Usage

1. Create a prefab with an `AudioSource` component (e.g. named `SoundFXPrefab`) and assign it in the inspector.
2. Create a Unity Audio Mixer and expose a SFX Group.
3. Attach this script to a GameObject in your scene and assign the mixer group.
4. Call from other scripts:

```csharp
SFXManagerAdvanced.Instance.PlaySFX(myClip, transform.position);
SFXManagerAdvanced.Instance.PlayRandomSFX(myClipsArray, Vector3.zero);
