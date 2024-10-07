# Slot Machine Prototype 4 - TJS (2021)

<table>
  <tr>
    <td><img src="https://github.com/user-attachments/assets/628d98c6-0224-48a2-b3e3-321b5f48e681" alt="InspectMe Logo" width="100"></td>
    <td>
      🛠️ Boost your Unity workflows with <a href="https://divinitycodes.de/">InspectMe</a>! Our tool simplifies debugging with an intuitive tree view. Check it out! 👉 
      <a href="https://assetstore.unity.com/packages/tools/utilities/inspectme-lite-advanced-debugging-code-clarity-283366">InspectMe Lite</a> - 
      <a href="https://assetstore.unity.com/packages/tools/utilities/inspectme-pro-advanced-debugging-code-clarity-256329">InspectMe Pro</a>
    </td>
  </tr>
</table>

---

## Introduction
This is the fourth experiment in a series where I explore different animation techniques for slot machines. For this prototype, I've employed my custom tween library, **[Tween Job System (TJS)](https://github.com/AirCoder89/TJS-Tween-Job-System-)**, which is an optimized animation engine based on a job system. The usage of TJS is quite similar to DOTween, with a range of handy shortcut extensions that simplify animation tasks in Unity.

**Video Demo**
[![CLICK HERE](https://github.com/user-attachments/assets/33cefed9-e105-4933-a1a2-757b63595b95)](https://www.youtube.com/watch?v=OIwOSg_UdRI)

## Animation
<table border="0">
  <tr>
    <td valign="top">
      <p>In this iteration, I opted for a smoother and more flexible animation approach by dividing the slots into columns and animating them, rather than animating each slot individually. This allows for more controlled and cohesive motion. To enhance the spinning illusion, additional slots are incorporated off-screen. I believe adding motion blur via a Shader could yield even more impressive results.</p>
    </td>
    <td>
      <img src="https://github.com/user-attachments/assets/38ef8ab5-83c3-460a-b69f-c946ac07e977" alt="Animation Example" width="1500">
    </td>
  </tr>
</table>


![image](https://github.com/user-attachments/assets/aef8c841-4cdd-4d77-898c-8fd435ef87a4)

<table border="0">
  <tr>
    <td>
      <img src="https://github.com/user-attachments/assets/2e7084b5-45fa-4b9e-9b88-67bd88144f2c" alt="Scriptable Objects" width="1000">
    </td>
    <td valign="top">
      <p>Animations and behaviors are encapsulated as "scriptable objects", making it easier to switch between different settings and behaviors. I've found that maintaining control over the animations allows for more varied and dynamic behavior in the gameplay.</p>
    </td>
  </tr>
</table>

![image](https://github.com/user-attachments/assets/fcc136af-bed4-476a-b5da-ae217de1cffa)



## Architecture
<table border="0">
  <tr>
    <td>
      <img src="https://github.com/user-attachments/assets/7667baa2-540e-4d7e-a16a-0ce620b081c0" alt="Architecture Diagram" width="1000">
    </td>
    <td valign="top">
      <p>The architecture for this project is inspired by the <a href="https://github.com/AirCoder89/Unity-AMVC-Design-Pattern">AMVC for UI</a> pattern, which I developed from my experiences with Unity UI projects. The core of this architecture is the <code>MachineController.cs</code>, which acts as both the entry point and a dependency container. This setup allows for precise control over initialization and update sequences of various components. It's a pattern I've used successfully in previous projects, such as my <a href="https://github.com/AirCoder89/Hybrid-ECS-Pixelart-Game-Prototype-">Pixelart Game [Unity Prototype]</a>.</p>
    </td>
  </tr>
</table>


## Addressable Assets System
This prototype demonstrates the basic use of Unity's Addressable Assets System. It includes both synchronous and asynchronous methods for loading and instantiating assets:
- **Synchronous**: Load the slot prefab asynchronously with Unity Addressable and then instantiate it synchronously.
- **Asynchronous**: Load and instantiate all slot instances asynchronously using Unity Addressable.

![image](https://github.com/user-attachments/assets/74115929-ebd0-4a7c-8349-5637da231a31)


## Audio
<table border="0">
  <tr>
    <td>
      <img src="https://github.com/user-attachments/assets/49fddf3b-7340-4f72-9387-2959c0d36ecd" alt="Audio System Diagram" width="1000">
    </td>
    <td valign="top">
      <p>The audio system is dynamically linked with the animation to enhance the gaming experience. I chose a random slot machine sound effect and developed a system that manages all sound effects. This system allows slicing a single audio clip into multiple pieces, which can be played with variations in pitch to differentiate the sounds among columns.</p>
    </td>
  </tr>
</table>

![image](https://github.com/user-attachments/assets/ea257a9d-9f84-4a8f-aab4-899f7041e1ed)


## Previous Prototypes
For those interested in the evolution of this project, here are links to the previous slot machine prototypes:
- [Experiment-ECS-SlotMachine](https://github.com/AirCoder89/Experiment-ECS-SlotMachine)
- [Experiment-SlotMachineV2](https://github.com/AirCoder89/Experiment-SlotMachineV2)
- [Experiment-SlotMachine-UV-Scrolling](https://github.com/AirCoder89/Experiment-SlotMachineUVScrolling)

Feel free to explore and provide feedback or contributions to improve the prototype!

---

<table>
  <tr>
    <td><img src="https://github.com/user-attachments/assets/628d98c6-0224-48a2-b3e3-321b5f48e681" alt="InspectMe Logo" width="100"></td>
    <td>
      🛠️ Boost your Unity workflows with <a href="https://divinitycodes.de/">InspectMe</a>! Our tool simplifies debugging with an intuitive tree view. Check it out! 👉 
      <a href="https://assetstore.unity.com/packages/tools/utilities/inspectme-lite-advanced-debugging-code-clarity-283366">InspectMe Lite</a> - 
      <a href="https://assetstore.unity.com/packages/tools/utilities/inspectme-pro-advanced-debugging-code-clarity-256329">InspectMe Pro</a>
    </td>
  </tr>
</table>
