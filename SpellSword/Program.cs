using System;
using System.Drawing;
using BearLib;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.Messaging;
using SpellSword.Actors;
using SpellSword.Actors.Action;
using SpellSword.Actors.Pathing;
using SpellSword.Engine;
using SpellSword.Engine.Components;
using SpellSword.Input;
using SpellSword.Logging;
using SpellSword.MapGeneration;
using SpellSword.Render;
using SpellSword.Render.Lighting;
using SpellSword.Render.Panes;
using SpellSword.RPG;
using SpellSword.RPG.Alignment;
using SpellSword.TestUtils;
using SpellSword.Time;
using GlyphComponent = SpellSword.Engine.Components.GlyphComponent;
using Rectangle = GoRogue.Rectangle;

namespace SpellSword
{
    class Program
    {
        static void Main(string[] args)
        {
            Timeline timeline = new Timeline();
            Map map = new BasicGenerator().Generate(120, 30);
            UpdatingGameObject.MainTimeline = timeline;
            MessageBus mainBus = new MessageBus();
            Spawner spawner = new Spawner(map);
            
            mainBus.RegisterSubscriber(spawner);

            JoystickConfig joystickConfig = new JoystickConfig
                { DownCode = Terminal.TK_S, LeftCode = Terminal.TK_A, RightCode = Terminal.TK_D, UpCode = Terminal.TK_W, 
                    MapUp = Terminal.TK_UP, MapDown = Terminal.TK_DOWN, MapRight = Terminal.TK_RIGHT, MapLeft = Terminal.TK_LEFT};

            LightMap lightMap = new LightMap(120, 30, map.TransparencyView);

            //lightMap.AddLight(new Light(Color.AntiqueWhite, new Coord(1, 1), 15, 5));
            lightMap.AddLight(new Light(Color.Wheat, new Coord(8, 8), 20, 10000));

            // Creating Player
            GameObject player = UpdatingGameObject.CreateUpdatingGameObject(new Coord(8, 8), Layers.Main, null);

            Being playerBeing = new Being(new SelectedAttributes(new AttributeSet(10, 10, 10, 10, 10)), Alignment.PlayerAlignment, new EquipmentSlotSet(), 10, "You");

            playerBeing.Equipment.Equip(new MeleeWeapon(new Damage(10)), EquipmentSlot.RightHandEquip);

            UserControlAgent control = new UserControlAgent(joystickConfig);
            mainBus.RegisterSubscriber<MouseMoveEvent>(control);
            mainBus.RegisterSubscriber<KeyEvent>(control);

            Actor playerActor = new Actor(playerBeing, control, mainBus);
            player.AddComponent(playerActor);
            player.AddComponent(new GlyphComponent(new Glyph('@', Color.Aqua)));
            player.AddComponent(new LightSourceComponent(lightMap, new Light(Color.Aqua, Coord.NONE, 6, 100)));
            player.AddComponent(new FOVExplorerComponent());

            Logger logger = new Logger();
            mainBus.RegisterSubscriber(logger);

            MapViewPane viewPane = new MapViewPane(map, lightMap, mainBus, joystickConfig);
            map.ObjectRemoved += (sender, eventArgs) => viewPane.SetDirty();
            map.ObjectMoved += (sender, eventArgs) => viewPane.SetDirty();
            map.ObjectAdded += (sender, eventArgs) => viewPane.SetDirty();


            TextPane descriptionPane = new TextPane("TEST TEXT");

            Describer describer = new Describer(descriptionPane, map);
            mainBus.RegisterSubscriber(describer);

            LogPane logPane = new LogPane(logger);

            StackPane mapAndConsole = new StackPane(StackPane.StackDirection.Vertical);
            mapAndConsole.AddChild(descriptionPane, 1);
            mapAndConsole.AddChild(viewPane, 4);
            mapAndConsole.AddChild(logPane, 1);
            
            StackPane statusBars = new StackPane(StackPane.StackDirection.Vertical);
            statusBars.AddChild(new FillBarPane(playerBeing.Health, "Health", Color.Red, Color.DarkRed), 1);
            statusBars.AddChild(new FillBarPane(playerBeing.Mana, "Mana", Color.Aqua, Color.DarkBlue), 1);
            statusBars.AddChild(new FillBarPane(playerBeing.Stamina, "Stamina", Color.Yellow, Color.DarkGoldenrod), 1);
            statusBars.AddChild(new InventoryDisplayPane(playerBeing.Equipment, playerBeing.Inventory, joystickConfig), 1);

            StackPane root = new StackPane(StackPane.StackDirection.Horizontal);
            root.AddChild(mapAndConsole, 4);
            root.AddChild(statusBars, 1);

            BearTermInputRouter inputRouter = new BearTermInputRouter(root);

            Window rootWindow = new Window(root, new Rectangle(0, 0, 140, 40), 0);

            BearTermRenderer renderer = new BearTermRenderer(rootWindow, "window.title='Spell Sword'; window.size=140x40; window.resizeable=true; input.filter=[keyboard+, mouse+, arrows+]");
            mainBus.RegisterSubscriber(renderer);

            GoalMapStore goalMapStore = new GoalMapStore(map);

            for (int i = 1; i < 5; i++)
            {
                GameObject goblin = TestUtil.CreateGoblin((5 + i, 5 + i % 2 + 1), playerActor, timeline, goalMapStore, mainBus);
                map.AddEntity(goblin);
            }

            map.AddEntity(player);

            Terminal.Refresh();
            

            int lastFrame = Environment.TickCount;

            while (true)
            {
                inputRouter.HandleInput();
                //control.VisualizeAim(playerActor);

                if (!control.WaitingForInput)
                {
                    timeline.Advance(1);
                    goalMapStore.UpdateMaps();
                }

                if (Environment.TickCount - lastFrame > 20)
                {
                    renderer.Refresh();
                    lastFrame = Environment.TickCount;
                }
            }
            
        }
    }
}
