import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';

import { ToDo } from '../shared/apiClient'

@Component({
  selector: 'app-todo-item',
  templateUrl: './todo-item.component.html',
  styleUrls: ['./todo-item.component.scss']
})
export class TodoItemComponent implements OnChanges {

  @Input() todo!: ToDo;
  @Output() todoChange = new EventEmitter(true)
  @Output() todoDelete = new EventEmitter(true)

  ngOnChanges(changes: SimpleChanges): void {
  }

  OnCheckedChanged(id: number, checkValue: boolean)
  {
    console.log("OnCheckedChanged:"+JSON.stringify(checkValue)+" id = "+id);
    this.todoChange.emit();
  }

  OnNameChanged(id: number, name: string)
  {
    console.log("OnNameChanged:"+JSON.stringify(name)+" id = "+id);
    this.todoChange.emit(this.todo.id);
  }

  OnDeleteClicked(id: number)
  {
    this.todoDelete.emit(this.todo.id);
  }
}
