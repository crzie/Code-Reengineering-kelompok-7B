using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public List<Ability> AbilityList;

    // Start is called before the first frame update
    void Start()
    {
        //AbilityList = new List<Ability>
        //{
        //    new("Horizontal Slash",
        //        "In a swift and precise motion, the wielder executes a powerful horizontal strike, cutting through enemies in a wide arc.",
        //        4000),
        //    new("Red Energy Explosion",
        //        "The user channels their energy, creating an expanding sphere of red-hot power. As the sphere reaches its peak, it violently explodes, releasing a shockwave of concussive energy that engulfs everything in its radius.",
        //        5000),
        //    new("Meteor Shower",
        //        "The user invokes a cosmic connection, summoning a cascade of meteors from the heavens. Each meteor hurtles down with tremendous force, creating fiery impact zones upon landing.",
        //        10000),
        //    new("Laser Rain",
        //        "Unleash a dazzling cascade of laser beams from above, raining down with precision and intensity. Each laser carries destructive energy, targeting foes with pinpoint accuracy. Whether in combat or as a dazzling display of power, the Laser Rain ability illuminates the battlefield, leaving opponents in awe and disarray.",
        //        15000),
        //    new("Hollow Red",
        //        "The reverse Limitless technique is powered by positive energy generated from reverse cursed technique as opposed to negative cursed energy. This reverses the effect of the strengthened Limitless, producing a strong repelling effect rather than an attracting one.",
        //        20000),
        //};
    }

}
