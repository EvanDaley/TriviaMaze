/* IActivate.cs
 * Author: Evan Daley
 * Revision: 0
 * Rev. Author: 
 * Description: Powerable objects should implement this class. 
 */

interface IActivate {

	StateEnum State
	{
		get;
		set;
	}
}
