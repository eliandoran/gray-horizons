using System;
using System.Xml.Serialization;

namespace BattleCity.Logic
{
	/// <summary>
	/// Represents a part of an in-game static object (like the four parts of a wall).
	/// </summary>
	public class Segment
	{
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="BattleCity.Logic.Segment"/> is intact.
		/// </summary>
		/// <value><c>true</c> if the segment is intact; otherwise, <c>false</c>.</value>
		[XmlAttribute("isIntact")]
		public bool IsIntact { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="BattleCity.Logic.Segment"/> class.
		/// </summary>
		public Segment (): this(true)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BattleCity.Logic.Segment"/> class indicating whether it is intact.
		/// </summary>
		/// <param name="isIntact"><c>true</c> if the segment is intact; otherwise, <c>false</c>.</param>
		public Segment(bool isIntact)
		{
			IsIntact = isIntact;
		}
	}
}

