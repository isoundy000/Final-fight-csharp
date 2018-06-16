﻿using Core.Math;
using System;
using System.Text;

namespace Core.XML
{
	public class XMLUtils
	{
		public static string DecodeString( string aSource )
		{
			int len = aSource.Length;
			StringBuilder sb = new StringBuilder();
			int pos1 = 0;

			while ( true )
			{
				int pos2 = aSource.IndexOf( '&', pos1 );
				if ( pos2 == -1 )
				{
					sb.Append( aSource.Substring( pos1 ) );
					break;
				}
				sb.Append( aSource.Substring( pos1, pos2 - pos1 ) );

				pos1 = pos2 + 1;
				pos2 = pos1;
				int end = MathUtils.Min( len, pos2 + 10 );
				for ( ; pos2 < end; pos2++ )
				{
					if ( aSource[pos2] == ';' )
						break;
				}
				if ( pos2 < end && pos2 > pos1 )
				{
					string entity = aSource.Substring( pos1, pos2 - pos1 );
					int u = 0;
					if ( entity[0] == '#' )
					{
						if ( entity.Length > 1 )
						{
							u = entity[1] == 'x' ? Convert.ToInt16( entity.Substring( 2 ), 16 ) : Convert.ToInt16( entity.Substring( 1 ) );
							sb.Append( ( char )u );
							pos1 = pos2 + 1;
						}
						else
							sb.Append( '&' );
					}
					else
					{
						switch ( entity )
						{
							case "amp":
								u = 38;
								break;

							case "apos":
								u = 39;
								break;

							case "gt":
								u = 62;
								break;

							case "lt":
								u = 60;
								break;

							case "nbsp":
								u = 32;
								break;

							case "quot":
								u = 34;
								break;
						}
						if ( u > 0 )
						{
							sb.Append( ( char )u );
							pos1 = pos2 + 1;
						}
						else
							sb.Append( '&' );
					}
				}
				else
				{
					sb.Append( '&' );
				}
			}

			return sb.ToString();
		}

		public static string EncodeString( string str )
		{
			if ( string.IsNullOrEmpty( str ) )
				return string.Empty;
			return str.Replace( "&", "&amp;" ).Replace( "<", "&lt;" ).Replace( ">", "&gt;" ).Replace( "'", "&apos;" );
		}
	}
}
