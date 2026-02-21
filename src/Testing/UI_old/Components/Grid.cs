namespace MonoGame.UI
{
	/// <summary>
	/// Contenedor con layout de cuadrícula
	/// </summary>
	public class Grid : Panel
    {
        public int Rows { get; set; } = 1;
        public int Columns { get; set; } = 1;
        public Spacing CellSpacing { get; set; } = new Spacing(0.01f);

        protected override void CalculateTransformations()
        {
            base.CalculateTransformations();

            if (Rows <= 0 || Columns <= 0 || Children.Count == 0) return;

            float cellWidth = (1f - Padding.Horizontal - (CellSpacing.Horizontal * (Columns - 1))) / Columns;
            float cellHeight = (1f - Padding.Vertical - (CellSpacing.Vertical * (Rows - 1))) / Rows;

            int childIndex = 0;
            for (int row = 0; row < Rows && childIndex < Children.Count; row++)
            {
                for (int col = 0; col < Columns && childIndex < Children.Count; col++)
                {
                    var child = Children[childIndex++];
                    
                    float x = Padding.Left + col * (cellWidth + CellSpacing.Horizontal);
                    float y = Padding.Top + row * (cellHeight + CellSpacing.Vertical);

                    child.RelativeBounds = new RectangleRelative(x, y, cellWidth, cellHeight);
                    child.Invalidate();
                }
            }
        }
    }
}
