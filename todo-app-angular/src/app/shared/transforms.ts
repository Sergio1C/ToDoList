import { ToDo } from './apiClient'

export interface ThreeNode<T> {
    id: number
    text: T,
    checked: boolean,
    items? : ThreeNode<T>[]
}

//{"id":1,"name":"купить продукты на борщ","parentId":null,"checked":false},
//{"id":4,"name":"отнести книги в библиотеку","parentId":null,"checked":false},
//{"id":5,"name":"Жюль Верн","parentId":4,"checked":false},
//{"id":6,"name":"Путешествие к центру Земли","parentId":5,"checked":false},
//{"id":7,"name":"Дети капитана Гранта","parentId":5,"checked":false}]

export function fromArrayToThree(todo: ToDo[]): ThreeNode<string>[]
{
    let tree: ThreeNode<string>[] = []
    todo.forEach(_todo => {

        let tree_item = {id: _todo.id, text: _todo.name, checked: _todo.checked}
        if (!_todo.parentId)
        {
            tree.push(tree_item)
        }
        else
        {
            let parent = findParentRecursively(tree, _todo.parentId)
            if (parent)
            {
                if (parent?.items)
                    parent.items.push(tree_item)
                else
                    parent.items = new Array<ThreeNode<string>>(tree_item);
            }
        }
   });

   return tree;
}

function findParentRecursively(items: ThreeNode<string>[], id: number) : ThreeNode<string> | undefined
{
    for (let index = 0; index < items.length; index++) {
        const element = items[index];

        if (element.id == id)
        {
           return element;
        }
        else if (element.items)
        {
            let parent = findParentRecursively(element.items, id);
            if (parent)
            {
                return parent;
            }
            continue;
        }
    }
    return undefined;
}
