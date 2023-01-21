class ApiClient
{
    private webApiHost = "https://localhost:56989/todo";

    getAll(): Promise<Array<ToDo>>
    {
        // eslint-disable-next-line prefer-const
        let result: ToDo[] = [];

        return fetch(this.webApiHost, { method: "GET", headers: {"Accept": "application/json"}}).then((response : Response) =>
        {
            if (response.ok)
            {
                return response.text();
            }

        }).then(text => {
            if (text != undefined)
            {
                const result200 = JSON.parse(text);
                if (Array.isArray(result200))
                {
                    for (const item of result200)
                    {
                        result.push(ToDo.fromJS(item));
                    }
                }

                return Promise.resolve(result);
            }
            return Promise.reject(result);
        })
    }

    update(todo: ToDo) : Promise<ToDo>
    {
        let result: ToDo

        return fetch(this.webApiHost, { method: "POST",
            body: JSON.stringify(todo),
            headers: {"Content-Type": "application/json"}}).then((response : Response) =>
        {
            if (response.ok)
            {
                return response.text();
            }

        }).then(text => {
            if (text != undefined)
            {
                const result200 = JSON.parse(text);
                if (result200 != undefined)
                {
                    result = ToDo.fromJS(result200);
                }
                return Promise.resolve(result);
            }
            return Promise.reject(result);
        });
    }

    delete(todo: ToDo) : Promise<boolean>
    {
        return fetch(this.webApiHost+"/"+todo.id, { method: "DELETE"}).then(response =>
            {
               if (response.ok)
                return Promise.resolve(true);
                else
                return Promise.resolve(false);
            });
    }
}

export const ApiToDo = new ApiClient();

export class ToDo {
    id: number;
    name:  string;
    parentId? : number | undefined;
    checked: boolean;

    constructor(id:number, name: string, parentId: number | undefined, checked: boolean) {
        this.id = id;
        this.name = name;
        this.parentId = parentId;
        this.checked = checked;
    }

    static fromJS(data: any) : ToDo
    {
        data = typeof data === 'object' ? data : {};
        const result = new ToDo(data["id"], data["name"], data["parentId"], data["checked"])
        return result;
    }
}
