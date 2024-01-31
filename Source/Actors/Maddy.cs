
namespace Celeste64;

public class Maddy : NPC
{
	public const string TALK_FLAG = "MADDY";

	private readonly Hair hair;
	private Color hairColor = 0xdb2c00;

	public Maddy() : base(Assets.Models["maddy"])
	{
		foreach (var mat in Model.Materials)
		{
			if (mat.Name == "Hair")
			{
				mat.Color = hairColor;
				mat.Effects = 0;
			}
			mat.Set("u_silhouette_color", hairColor);
		}

        hair = new()
        {
            Color = hairColor,
			ForwardOffsetPerNode = 0,
            Nodes = 10
        };

		Model.Transform = Matrix.CreateScale(3) * Matrix.CreateTranslation(0, 0, -1.5f);
		InteractHoverOffset = new Vec3(0, -2, 16);
		InteractRadius = 32;
		CheckForDialog();
	}

    public override void Interact(Player player)
	{
		World.Add(new Cutscene(Conversation));
	}

	private CoEnumerator Conversation(Cutscene cs)
	{
		yield return Co.Run(cs.MoveToDistance(World.Get<Player>(), Position.XY(), 16));
		yield return Co.Run(cs.FaceEachOther(World.Get<Player>(), this));

		int index = Save.CurrentRecord.GetFlag(TALK_FLAG) + 1;
		yield return Co.Run(cs.Say(Assets.Dialog[$"Maddy{index}"]));
		Save.CurrentRecord.IncFlag(TALK_FLAG);
		CheckForDialog();
	}

    public override void CollectModels(List<(Actor Actor, Model Model)> populate)
    {
		populate.Add((this, hair));
        base.CollectModels(populate);
    }

	private void CheckForDialog()
	{ 
		InteractEnabled = Assets.Dialog.ContainsKey($"Maddy{Save.CurrentRecord.GetFlag(TALK_FLAG) + 1}");
	}
}

