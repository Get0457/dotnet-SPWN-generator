﻿// Where's public static void Main() Thing? If you're confused: https://aka.ms/new-console-template

using SPWNCreator;
using static SPWN.Basics.Extensions;
using static SPWN.Basics.SPWNUtils;

// Example 1

// Create and Store value in C# variable to Reuse the same value

var group1 = new SPWN.DataTypes.Group(1);

// Note: SPWN.DataTypes.Group.FromId() is the same thing as new SPWN.DataTypes.Group() one
var group2 = SPWN.DataTypes.Group.FromId(2);
var group3 = SPWN.DataTypes.Group.FromId(3);
var group4 = SPWN.DataTypes.Group.FromId(4);

Generator.PrintToConsole(
    Codes:
    // SPWN.Basics.SPWNCodes are basically list of SPWN codes.
    // Most SPWN-related code will return ISPWNCode, which is basically
    // a generated string of 1 statement of SPWN code.
    new SPWN.Basics.SPWNCodes
    {
        // To Generate comment in SPWN, use Comment(string s), Part of SPWN.Basics.SPWNUtils
        // To Create New Line, use NewLine(uint times = 1), Part of SPWN.Basics.SPWNUtils
        Comment("Example 1"),
        NewLine(),

        // Call the methods regularly
        group1.Follow(group2, Duration: 10),
        group2.Follow(group4),

        // To run a statement parallelly, use RunParallel(ISPWNCode code), Part of SPWN.Basics.SPWNUtils
        RunParallel(group1.Follow(group3))
    });

/* Code Output:
// Example 1

1g.follow(other = 2g, duration = 10)
2g.follow(other = 4g)
-> 1g.follow(other = 3g)
*/

// Example 2
// Reference: https://spu7nix.net/spwn/#/triggerlanguage/7selectorpanel

// You can implicitly casting C# Array into SPWN.DataTypes.Array, just like this
SPWN.DataTypes.Array<SPWN.DataTypes.Group> ArrayOfGroups = new SPWN.DataTypes.Group[] { 1, 2, 3, 4, 5, 6 };

Generator.PrintToConsole(
    Codes:
    new SPWN.Basics.SPWNCodes
    {
        Comment("Example 2"),
        NewLine(),

        Comment("Groups of the objects that decide the position of"),
        Comment("our buttons"),

        // If you want to create constant (non-mutable) variable in SPWN, you need to use CreateConstantVariable<T>(string VariableName, out T Variable, T Value)
        // Note: CreateVariable means constant variables
        // VariableName is the variable name in SPWN
        CreateConstantVariable("anchors", out var anchors, ArrayOfGroups),

        NewLine(),
        Comment("Group of the object that indicates which"),
        Comment("button is currently selected"),

        // You can ALSO Create SPWN variable without initializing value on the top
        CreateConstantVariable("selector", out var selector, SPWN.DataTypes.Group.FromId(7)),

        NewLine(),

        // Importing Libraries is basically the same, create the variable that gets the return value of Gamescence
        CreateConstantVariable("gs", out var gs, new SPWN.Libraries.Gamescene()),

        NewLine(),

        Comment("starts at first button (index 0)"),
        
        
        CreateConstantVariable("selected",out var selected, SPWN.DataTypes.Counter.FromNumber(0)),

        NewLine(),

        // To get the value from the variable, use variable.Value
        gs.ButtonA().OnTriggered(new SPWN.DataTypes.TriggerFunction( new SPWN.Basics.SPWNCodes
        {
            Comment("switch"),
            selected.Add(1),

            SPWN.Conditions.If(Expr: selected >= anchors.Length.AsValue(),
                Do: selected.Reset().End()
            ),

            Comment("convert selected to a normal number"),

            CreateConstantVariable("current_anchor", out var current_anchor,
                anchors[selected.ToConst((0,anchors.Length.AsValue())).AsValue()].AsValue()),

            selector.MoveTo(current_anchor)
        }))
    }
);
/* Code Output:
// Example 2

// Groups of the objects that decide the position of
// our buttons
anchors = [1g,2g,3g,4g,5g,6g]

// Group of the object that indicates which
// button is currently selected
selector = 7g

gs = import gamescene

// starts at first button (index 0)
selected = counter(0)

gs.button_a().on_triggered(function = !{
    // switch
    selected.add(num = 1)
    if selected >= anchors.length {
        selected.reset()
    }
    // convert selected to a normal number
    current_anchor = anchors[selected.to_const(0..anchors.length)]
    selector.move_to(target = current_anchor)
})
*/
