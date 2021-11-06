using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Protobuf;
using static UFE.UFEField.Types.UFEFieldLocation;
using static UFE.UFEField.Types.UFEFieldType;
using UFEFieldLocation = UFE.UFEField.Types.UFEFieldLocation;

namespace UFE.UFEedClient
{
	/// <summary>
	/// UFE message wrapper class
	/// </summary>
	// ReSharper disable once InconsistentNaming
	public class UFEMessage
	{
		private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1);

		/// <summary>
		/// Status class to hold long status
		/// </summary>
		public class Status
		{
			public long Long { get; set; }
		}
		
		/// <summary>
		/// UFE Message builder class
		/// </summary>
		public class Builder
		{
			/// <summary>
			/// Constructs UFE message builder
			/// </summary>
			/// <param name="wm">Optional <c>WireMessage</c> to construct from</param>
			public Builder(WireMessage wm = null)
			{
				WireMessage = wm ?? new WireMessage();
			}

			/// <summary>
			/// Underlying <c>WireMessage</c> readonly property
			/// </summary>
			public WireMessage WireMessage { get; }
			
			/// <summary>
			/// Message name property
			/// </summary>
			public string Name
			{
				get => WireMessage.Name;
				set => WireMessage.Name = value;
			}

			/// <summary>
			/// Sets message name
			/// </summary>
			/// <param name="name">name to set</param>
			/// <returns>self</returns>
			public Builder SetName(string name)
			{
				Name = name;
				return this;
			}

			/// <summary>
			/// Message long name property
			/// </summary>
			public string LongName
			{
				get => WireMessage.Longname;
				set => WireMessage.Longname = value;
			}

			/// <summary>
			/// Sets message long name
			/// </summary>
			/// <param name="longName">long name to set</param>
			/// <returns>self</returns>
			public Builder SetLongName(string longName)
			{
				LongName = longName;
				return this;
			}
			
			/// <summary>
			/// Message type property
			/// </summary>
			public WireMessage.Types.Type Type
			{
				get => WireMessage.Type; 
				set => WireMessage.Type = value;
			}

			/// <summary>
			/// Sets message type
			/// </summary>
			/// <param name="type">type to set</param>
			/// <returns>self</returns>
			public Builder SetType(WireMessage.Types.Type type)
			{
				Type = type;
				return this;
			}
			
			/// <summary>
			/// Message service id property
			/// </summary>
			public int ServiceId
			{
				get => WireMessage.ServiceId; 
				set => WireMessage.ServiceId = value;
			}

			/// <summary>
			/// Sets service id
			/// </summary>
			/// <param name="serviceId">Service id to set</param>
			/// <returns>self</returns>
			public Builder SetServiceId(int serviceId)
			{
				ServiceId = serviceId;
				return this;
			}

			/// <summary>
			/// Message subservice id property
			/// </summary>
			public int SubServiceId
			{
				get => WireMessage.SubserviceId; 
				set => WireMessage.SubserviceId = value;
			}

			/// <summary>
			/// Sets subservice id
			/// </summary>
			/// <param name="subServiceId">Subservice id to set</param>
			/// <returns>self</returns>
			public Builder SetSubServiceId(int subServiceId)
			{
				SubServiceId = subServiceId;
				return this;
			}

			/// <summary>
			/// Message sequence property
			/// </summary>
			public uint Seq
			{
				get => WireMessage.Seq;
				set => WireMessage.Seq = value;
			}

			/// <summary>
			/// Sets sequence
			/// </summary>
			/// <param name="seq"></param>
			/// <returns>self</returns>
			public Builder SetSeq(uint seq)
			{
				Seq = seq;
				return this;
			}
			
			/// <summary>
			/// Adds field with long value
			/// </summary>
			/// <param name="tag">Field tag</param>
			/// <param name="val">Field long value</param>
			/// <param name="loc">Field location</param>
			/// <returns>self</returns>
			public Builder AddField(uint tag, long val, UFEFieldLocation loc = FlBody)
			{
				WireMessage.Fields.Add(new UFEField {Tag = tag, Type = FtInt, Ival = val, Location = loc});
				return this;
			}

			/// <summary>
			/// Adds field with <c>ByteString</c> value
			/// </summary>
			/// <param name="tag">Field tag</param>
			/// <param name="val">Field <c>ByteString</c> value</param>
			/// <param name="loc">Field location</param>
			/// <returns>self</returns>
			public Builder AddField(uint tag, ByteString val, UFEFieldLocation loc = FlBody)
			{
				WireMessage.Fields.Add(new UFEField {Tag = tag, Type = FtString, Sval = val, Location = loc});
				return this;
			}

			/// <summary>
			/// Adds field with string value
			/// </summary>
			/// <param name="tag">Field tag</param>
			/// <param name="val">Field string value</param>
			/// <param name="loc">Field location</param>
			/// <returns>self</returns>
			public Builder AddField(uint tag, string val, UFEFieldLocation loc = FlBody)
			{
				WireMessage.Fields.Add(new UFEField {Tag = tag, Type = FtString, Sval = ByteString.CopyFrom(val, Encoding.Default), Location = loc});
				return this;
			}

			/// <summary>
			/// Adds field with char value
			/// </summary>
			/// <param name="tag">Field tag</param>
			/// <param name="val">Field string value</param>
			/// <param name="loc">Field location</param>
			/// <returns>self</returns>
			public Builder AddField(uint tag, char val, UFEFieldLocation loc = FlBody)
			{
				return AddField(tag, ByteString.CopyFrom(new[] {(byte)val}), loc);
			}

			/// <summary>
			/// Adds field with double value
			/// </summary>
			/// <param name="tag">Field tag</param>
			/// <param name="val">Field double value</param>
			/// <param name="loc">Field location</param>
			/// <param name="precision">Float precision</param>
			/// <returns>self</returns>
			public Builder AddField(uint tag, double val, UFEFieldLocation loc = FlBody, int precision = Consts.UFE_FLOAT_PRECISION)
			{
				WireMessage.Fields.Add(new UFEField {Tag = tag, Type = FtDouble, Fval = val, Ival = precision, Location = loc});
				return this;
			}

			/// <summary>
			/// Adds field with bool value
			/// </summary>
			/// <param name="tag">Field tag</param>
			/// <param name="val">Field double value</param>
			/// <param name="loc">Field location</param>
			/// <returns>self</returns>
			public Builder AddField(uint tag, bool val, UFEFieldLocation loc = FlBody)
			{
				WireMessage.Fields.Add(new UFEField {Tag = tag, Type = FtBool, Bval = val, Location = loc});
				return this;
			}

			/// <summary>
			/// Adds field with Datetime value
			/// </summary>
			/// <param name="tag">Field tag</param>
			/// <param name="val">Field DateTime value</param>
			/// <param name="loc">Field location</param>
			/// <returns>self</returns>
			public Builder AddField(uint tag, DateTime val, UFEFieldLocation loc = FlBody)
			{
				WireMessage.Fields.Add(new UFEField{Tag = tag, Type = FtTime, Ival = (long) ((val - UnixEpoch).TotalMilliseconds * 1000000L), Location = loc});
				return this;
			}

			/// <summary>
			/// Adds field with GUID value
			/// </summary>
			/// <param name="tag">Field tag</param>
			/// <param name="val">Field GUID value</param>
			/// <param name="loc">Field location</param>
			/// <returns>self</returns>
			public Builder AddField(uint tag, Guid val, UFEFieldLocation loc = FlBody)
			{
				WireMessage.Fields.Add(new UFEField{Tag = tag, Type = FtUuid, Sval = ByteString.CopyFrom(val.ToByteArray()), Location = loc});
				return this;
			}

			/// <summary>
			/// Adds field with status value
			/// </summary>
			/// <param name="tag">Field tag</param>
			/// <param name="val">Field status value</param>
			/// <param name="loc">Field location</param>
			/// <returns>self</returns>
			public Builder AddField(uint tag, Status val, UFEFieldLocation loc = FlBody)
			{
				WireMessage.Fields.Add(new UFEField{Tag = tag, Type = FtStatus, Ival = val.Long, Location = loc});
				return this;
			}

			/// <summary>
			/// Adds field with value boxed to object
			/// </summary>
			/// <param name="tag">Field tag</param>
			/// <param name="val">Field object value</param>
			/// <param name="loc">Field location</param>
			/// <returns>self</returns>
			public Builder AddField(uint tag, object val, UFEFieldLocation loc = FlBody)
			{
				switch (val)
				{
					case char c:
						AddField(tag, c, loc);
						break;
					case string s:
						AddField(tag, s, loc);
						break;
					case double d:
						AddField(tag, d, loc);
						break;
					case long l:
						AddField(tag, l, loc);
						break;
					case int i:
						AddField(tag, i, loc);
						break;
					case bool y:
						AddField(tag, y, loc);
						break;
					case ByteString b:
						AddField(tag, b, loc);
						break;
					case DateTime t:
						AddField(tag, t, loc);
						break;
					case Guid g:
						AddField(tag, g, loc);
						break;
					case Status st:
						AddField(tag, st, loc);
						break;
					default:
						throw new UFEedClientException($"object type is unsupported, '{val}'");
				}
				return this;
			}

			/// <summary>
			/// Adds a collection of fields
			/// </summary>
			/// <param name="fields">fields enumerable to add</param>
			/// <returns>self</returns>
			public Builder AddFields(IEnumerable<UFEField> fields)
			{
				WireMessage.Fields.AddRange(fields as UFEField[] ?? fields.ToArray());
				return this;
			}
				
			/// <summary>
			/// Adds a group
			/// </summary>
			/// <param name="to">field to add a message to</param>
			/// <param name="tag">Field tag</param>
			/// <param name="group">Group field, out</param>
			/// <param name="tr">Group creation action</param>
			/// <param name="loc">Field location</param>
			/// <returns>self</returns>
			public Builder AddGroup(uint tag, out UFEField group, Action<Builder, UFEField> tr, UFEFieldLocation loc = FlBody)
			{
				group = new UFEField {Tag = tag, Type = FtMsg};
				WireMessage.Fields.Add(group);
				tr?.Invoke(this, group);
				group.Ival = group.Mval.Count;
				return this;
			}

			public Builder AddGroupItem(UFEField group)
			{
				var wm = new WireMessage();
				group.Mval.Add(wm);
				return new Builder(wm);
			}

			/// <summary>
			/// Creates UFEMessage from builder 
			/// </summary>
			/// <returns>built UFEMessage</returns>
			public UFEMessage Build()
			{
				return new UFEMessage(WireMessage); 
			}

			/// <summary>
			/// Prints message content
			/// </summary>
			/// <returns>string with printed message content</returns>
			public string Print()
			{
				return PrintWm(WireMessage);
			}
			
			public static string PrintWm(WireMessage wm, int depth = 0)
			{
				var dspacer = new string(' ', (1 + depth) * 3);
				var sspacer = "   ";
				var sb = new StringBuilder();
				sb .Append(new string(' ', (depth * 3)))
					.Append("srvc_id=").Append(wm.ServiceId)
					.Append(" subsrvc_id=").Append(wm.SubserviceId)
					.Append(" type=").Append(wm.Type);
				if (!string.IsNullOrEmpty(wm.Name))
					sb.Append(" msg=").Append(wm.Name);
				if (!string.IsNullOrEmpty(wm.Longname))
					sb.Append(" (").Append(wm.Longname).Append(')');
				sb.Append(" seq=").Append(wm.Seq).Append('\n');
				foreach(var f in wm.Fields) 
				{
					sb.Append(dspacer).Append(f.Tag).Append(" (");
					switch (f.Location) 
					{
						case FlBody   : sb.Append("body"); break;
						case FlHeader : sb.Append("hdr" ); break;
						case FlTrailer: sb.Append("trl" ); break;
						case FlSystem : sb.Append("sys" ); break;
						default       : sb.Append("unknown" ); break;
					}
					sb.Append("): ");
					switch (f.Type) {
						case FtInt:
							sb.Append(sspacer).Append(f.Ival).Append('\n');
							break;
						case FtChar:
							sb.Append(sspacer).Append(f.Sval[0]).Append('\n');
							break;
						case FtDouble:
							sb.Append(sspacer).Append(f.Fval).Append(" (").Append(f.Ival).Append(")\n");
							break;
						case FtString:
							sb.Append(sspacer).Append(f.Sval.ToStringUtf8()).Append('\n');
							break;
						case FtBool:
							sb.Append(sspacer).Append(f.Bval ? 'Y' : 'N').Append('\n');
							break;
						case FtTime:
							sb .Append(sspacer)
								.Append((UnixEpoch + TimeSpan.FromMilliseconds(f.Ival / 1000000.0)).ToString("O")).Append('\n');;
							break;
						case FtUuid:
							sb .Append(sspacer)
								.Append(new Guid(f.Sval.ToByteArray()).ToString()).Append('\n');;
							break;
						case FtStatus:
							sb.Append(sspacer).Append("status(").Append(f.Ival).Append(')').Append('\n');
							break;
						case FtMsg:
							sb.Append(sspacer).Append(f.Ival).Append(" elements, depth=").Append(depth).Append(" ... ").Append('\n');
							foreach(var m in f.Mval) {
								sb.Append(PrintWm(m, depth + 1));
							}
							break;
						default:
							sb.Append("Unknown type:").Append(f.Type).Append('\n');
							break;
					}
				}
				return sb.ToString();
			}
		}
		
		/// <summary>
		/// Constructs UFE message from <c>WireMessage</c>
		/// </summary>
		/// <param name="wm"><c>WireMessage</c> to construct from</param>
		private UFEMessage(WireMessage wm)
		{
			WireMessage = new WireMessage(wm);
			RemapWireMessage();
		}

		/// <summary>
		/// Message name property
		/// </summary>
		public string Name => WireMessage.Name;

		/// <summary>
		/// Message long name property
		/// </summary>
		public string LongName => WireMessage.Name;

		/// <summary>
		/// Message service id property
		/// </summary>
		public int ServiceId => WireMessage.ServiceId;

		/// <summary>
		/// Message subservice id property
		/// </summary>
		public int SubServiceId => WireMessage.SubserviceId;

		/// <summary>
		/// Message sequence property
		/// </summary>
		public uint Seq => WireMessage.Seq;

		/// <summary>
		/// Creates a new builder
		/// </summary>
		/// <returns>message builder from this message</returns>
		public Builder NewBuilder()
		{
			return new Builder(WireMessage); 
		}
		
		/// <summary>
		/// Underlying <c>WireMessage</c> readonly property
		/// </summary>
		public WireMessage WireMessage { get; }

		/// <summary>
		/// Mapped fields property
		/// </summary>
		public Dictionary<uint, UFEField> Fields { get; } = new Dictionary<uint, UFEField>();

		/// <summary>
		/// Mapped groups property
		/// </summary>
		public Dictionary<uint, List<UFEMessage>> Groups { get; } = new Dictionary<uint, List<UFEMessage>>();

		/// <summary>
		/// Finds field by tag
		/// </summary>
		/// <param name="tag">field tag to find</param>
		/// <returns>Found field or null if not found</returns>
		public UFEField FindField(uint tag)
		{
			Fields.TryGetValue(tag, out var field);
			return field;
		}

		/// <summary>
		/// Finds field by tag
		/// </summary>
		/// <param name="tag">field tag to find</param>
		/// <returns>Found field or null if not found</returns>
		public object FindFieldValue(uint tag)
		{
			var field = FindField(tag);
			if (field == null)
				return null;
			switch (field.Type)
			{
				case FtUnknown:
					return null;
				case FtInt:
					return field.Ival;
				case FtChar:
					return field.Sval[0];
				case FtDouble:
					return field.Fval;
				case FtString:
					return field.Sval.ToStringUtf8();
				case FtBool:
					return field.Bval;
				case FtTime:
					return UnixEpoch + TimeSpan.FromMilliseconds(field.Ival / 1000000.0); 
				case FtMsg:
					return Groups[field.Tag];
				case FtUuid:
					return new Guid(field.Sval.ToByteArray());
				case FtStatus:
					return new Status{ Long = field.Ival };
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		/// <summary>
		/// Finds group by tag
		/// </summary>
		/// <param name="tag">group tag to find</param>
		/// <returns>Found group or null if not found</returns>
		public List<UFEMessage> FindGroup(uint tag)
		{
			Groups.TryGetValue(tag, out var group);
			return group;
		}

		/// <summary>
		/// Prints message content
		/// </summary>
		/// <returns>string with printed message content</returns>
		public string Print()
		{
			return Builder.PrintWm(WireMessage);
		}

		private void RemapField(UFEField field)
		{
			if (field.Type == FtMsg)
			{
				if (!Groups.ContainsKey(field.Tag))
					Groups[field.Tag] = new List<UFEMessage>();
				var grp = Groups[field.Tag];
				foreach (var mval in field.Mval)
					grp.Add(new UFEMessage(mval));
			}
			Fields[field.Tag] = field;
		}

		private void RemapWireMessage()
		{
			foreach (var field in WireMessage.Fields)
				RemapField(field);
		}
	}
}
