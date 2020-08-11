using Coldairarrow.Entity.PB;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.PD
{
	public partial class PD_Plan
	{
		[ForeignKey(nameof(MaterialId))]
		public PB_Material Material { get; set; }
	}
}
