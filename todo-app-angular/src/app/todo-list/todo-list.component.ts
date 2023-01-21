import { Component, OnInit } from '@angular/core';
import { ApiToDo, ToDo } from '../shared/apiClient'
import { fromArrayToThree, ThreeNode } from '../shared/transforms'

import { TreeItemLookup } from '@progress/kendo-angular-treeview'

@Component({
  selector: 'app-todo-list',
  templateUrl: './todo-list.component.html',
  styleUrls: ['./todo-list.component.scss']
})
export class TodoListComponent implements OnInit {

  todo_arr : ToDo[] = []
  todo_three : ThreeNode<string>[] = []
  checkedKeys	:any[] = []
  checkBy : string = "id"

  async ngOnInit(eventName: string = "ngOnInit") {
    console.log(eventName);
    this.todo_arr = await ApiToDo.getAll();
    this.todo_three = fromArrayToThree(this.todo_arr);
    this.checkedKeys = this.todo_arr.filter(it => it.checked == true).map(it => it.id);
    console.log(JSON.stringify(this.checkedKeys));
  }

  async checkedChange(item: TreeItemLookup)
  {
    let id = item.item.dataItem[this.checkBy];
    let checked = this.checkedKeys.findIndex(it => it == id) != -1;

    let todo = this.getTodoById(id);
    if (todo)
    {
      todo.checked = checked;
      await this.Change(todo.id);
    }
  }

  getTodoById(id: number)
  {
    let todo = this.todo_arr.find(it => it.id == id);
    return todo ?? new ToDo(0,"",undefined,false);
  }

  //CRUD
  async Create(parentId: number | undefined)
  {
    await ApiToDo.update(new ToDo(0, "", parentId, false));
    await this.ngOnInit("Create");
  }

  async Change(id: number)
  {
    let ToDoToUpdate = this.getTodoById(id);
    await ApiToDo.update(ToDoToUpdate);
  }

  async Delete(id: number)
  {
    let ToDoToDelete = this.getTodoById(id);
    await ApiToDo.delete(ToDoToDelete);
    await this.ngOnInit("Delete");
  }
}
