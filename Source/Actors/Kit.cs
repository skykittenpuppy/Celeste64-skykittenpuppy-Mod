
namespace Celeste64;

public class Kit : NPC
{
	public readonly string Conversation = "CreditsKit";

	public Kit() : base(Assets.Models["kit"])
	{
		Model.Transform = Matrix.CreateScale(3) * Matrix.CreateTranslation(0, 0, -1.5f);
		InteractHoverOffset = new Vec3(0, -2, 16);
		InteractRadius = 32;
	}

    public override void Interact(Player player)
	{
		World.Add(new Cutscene(Talk));
	}

	private CoEnumerator Talk(Cutscene cs)
	{
		yield return Co.Run(cs.Face(World.Get<Player>(), Position));
		yield return Co.Run(cs.Say(Loc.Lines(Conversation)));
	}
}

