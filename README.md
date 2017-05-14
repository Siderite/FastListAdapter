# FastListAdapter
There is a bad design issue in the way UpdatePanels postback asynchronously select elements, making them very slow when the page contains large ListBox or DropDownList controls. This adapter fixes the problem.
