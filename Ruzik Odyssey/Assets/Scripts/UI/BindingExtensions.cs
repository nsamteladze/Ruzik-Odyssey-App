using RuzikOdyssey.Common;

namespace RuzikOdyssey.UI
{
	public static class BindingExtensions
	{
		public static UiLabelBindingBuilderSyntax Bind(this UILabel label)
		{
			return new UiLabelBindingBuilderSyntax(label);
		}

		public static void To<TSource>(this UiLabelBindingBuilderSyntax builderSyntax, Property<TSource> property)
		{
			builderSyntax.target.text = property.Value.ToString();
			property.PropertyChanged += (sender, e) => builderSyntax.target.text = e.PropertyValue.ToString();
		}

		public static void BindTo<TSource>(this UILabel label, Property<TSource> property)
		{
			property.PropertyChanged += (sender, e) => label.text = e.PropertyValue.ToString();
		}
	}

	public class UiLabelBindingBuilderSyntax
	{
		public readonly UILabel target;

		public UiLabelBindingBuilderSyntax(UILabel target)
		{
			Log.Debug("UiLabelBindingBuilderSyntax constructor");
			this.target = target;
		}
	}
}