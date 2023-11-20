public class MazePath
{
    public List<PathItem> Items { get; } = new List<PathItem>();
    public bool Solved { get; set; }
    public Nullable<double> ElapsedMilliseconds { get; set; } = null;

    public override bool Equals(object obj)
    {
        if (obj is MazePath path)
        {
            if (Items.Count != path.Items.Count)
                return false;

            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].X != path.Items[i].X || Items[i].Y != path.Items[i].Y)
                    return false;
            }

            return true;
        }
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
