using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EW.EWCode.Vfx
{
    public static class HLZYAttackVfx
    {
        private const string HLZYNodePrefix = "HLZYCompanion";
        private const float BeamSeconds = 0.42f;

        private static readonly Color EdgeColor = new(0.02f, 0.0f, 0.0f, 0.88f);
        private static readonly Color CoreColor = new(1.0f, 0.02f, 0.0f, 1.0f);
        private static readonly Color SparkColor = new(0.9f, 0.0f, 0.0f, 0.72f);

        public static void PlayFromAllHLZYTo(Creature? target)
        {
            PlayFromAllHLZYTo(null, target);
        }

        public static void PlayFromAllHLZYTo(Creature? owner, Creature? target)
        {
            try
            {
                MainFile.Logger.Info($"HLZY attack VFX request received. target={(target == null ? "null" : target.LogName)}.");
                PlayFromAllHLZYToInternal(owner, target);
            }
            catch (Exception ex)
            {
                MainFile.Logger.Info($"HLZY attack VFX skipped after error: {ex.Message}");
            }
        }

        private static void PlayFromAllHLZYToInternal(Creature? owner, Creature? target)
        {
            var room = NCombatRoom.Instance;
            if (room == null || target == null)
            {
                MainFile.Logger.Info($"HLZY attack VFX skipped: room null={room == null}, target null={target == null}.");
                return;
            }

            var targetNode = room.GetCreatureNode(target);
            var vfxContainer = room.CombatVfxContainer;
            if (targetNode == null || vfxContainer == null)
            {
                MainFile.Logger.Info($"HLZY attack VFX skipped: target node null={targetNode == null}, vfx container null={vfxContainer == null}.");
                return;
            }

            var targetPosition = targetNode.VfxSpawnPosition;
            if (targetPosition == Vector2.Zero)
            {
                targetPosition = targetNode.Body?.GlobalPosition ?? targetNode.GlobalPosition;
            }
            Node? summonRoot = owner == null ? room : room.GetCreatureNode(owner);
            if (summonRoot == null)
            {
                MainFile.Logger.Info("HLZY attack VFX skipped: owner node was not found.");
                return;
            }

            var summons = FindHLZYSummonNodes(summonRoot).ToList();
            MainFile.Logger.Info($"HLZY attack VFX: summons={summons.Count}, target={targetPosition}.");

            foreach (var hlzy in summons)
            {
                PlayBeam(vfxContainer, hlzy.GlobalPosition + new Vector2(0f, -52f), targetPosition);
            }
        }

        private static IEnumerable<Node2D> FindHLZYSummonNodes(Node root)
        {
            foreach (var child in root.GetChildren())
            {
                if (child is Node2D node2D && node2D.Name.ToString().StartsWith(HLZYNodePrefix, StringComparison.Ordinal))
                {
                    yield return node2D;
                }

                if (child is Node childNode)
                {
                    foreach (var nested in FindHLZYSummonNodes(childNode))
                    {
                        yield return nested;
                    }
                }
            }
        }

        private static void PlayBeam(Control parent, Vector2 startGlobal, Vector2 endGlobal)
        {
            var toLocal = parent.GetGlobalTransformWithCanvas().AffineInverse();
            var start = toLocal * startGlobal;
            var end = toLocal * endGlobal;

            var beam = new Node2D
            {
                Name = "HLZYAttackBeam",
                ZIndex = 900
            };

            parent.AddChild(beam);

            var edge = CreateLine(start, end, 34f, EdgeColor);
            var core = CreateLine(start + new Vector2(2f, -2f), end + new Vector2(2f, -2f), 13f, CoreColor);
            beam.AddChild(edge);
            beam.AddChild(core);

            var direction = start.DirectionTo(end);
            var normal = direction.Orthogonal();
            for (var i = 0; i < 5; i++)
            {
                var t = (i + 1f) / 6f;
                var center = start.Lerp(end, t) + normal * (GD.Randf() * 24f - 12f);
                var length = 12f + GD.Randf() * 16f;
                var spark = CreateLine(
                    center - direction * length * 0.5f,
                    center + direction * length * 0.5f,
                    2f + GD.Randf() * 2f,
                    SparkColor
                );
                beam.AddChild(spark);
            }

            var tween = parent.GetTree()?.CreateTween();
            if (tween == null)
            {
                beam.QueueFree();
                return;
            }

            tween.SetParallel(true);
            tween.SetTrans(Tween.TransitionType.Quad);
            tween.SetEase(Tween.EaseType.Out);

            foreach (var child in beam.GetChildren())
            {
                if (child is not Line2D line)
                {
                    continue;
                }

                tween.TweenProperty(line, "width", 0.0, BeamSeconds);
                tween.TweenProperty(line, "modulate:a", 0.0, BeamSeconds);
            }

            tween.Chain().TweenCallback(Callable.From(beam.QueueFree));
        }

        private static Line2D CreateLine(Vector2 start, Vector2 end, float width, Color color)
        {
            return new Line2D
            {
                Points = [start, end],
                Width = width,
                DefaultColor = color,
                JointMode = Line2D.LineJointMode.Round,
                BeginCapMode = Line2D.LineCapMode.Round,
                EndCapMode = Line2D.LineCapMode.Round
            };
        }
    }
}
