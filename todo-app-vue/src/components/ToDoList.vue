<template>
      <v-container>
        <v-btn @click="Create(null)">New task</v-btn>
        <v-btn :disabled="$data.active == 0" @click="Create($data.active)"
          >New subtask</v-btn
        >
        <v-treeview
          selectable
          activatable
          open-all
          :items="$data.tree"
          v-model="$data.selection"
          :active="[$data.active]"
          @update:active="
            (arr) => {
              this.$data.active = arr.pop() || 0;
            }
          "
        >
          <template #label="{ item, selected }">
            <div :class="{ 'text-crossed': selected }">
              <v-input>
                <v-text-field
                  v-model="item.name"
                  @change="ChangeName(item.id, item.name)"
                >
                </v-text-field>
              </v-input>
            </div>
          </template>
          <template #append="{ item }">
            <v-btn
              :key="item.id"
              @click="Delete(item.id)"
              :disabled="HasChildren(item.id)"
              >Delete{{ item.id }}</v-btn
            >
          </template>
        </v-treeview>
      </v-container>
</template>

<script lang="ts">
import { Component, Vue, Watch } from "vue-property-decorator";
import { ApiToDo, ToDo } from "./../ApiClient";
import { arrayToTree, TreeItem } from "performant-array-to-tree";

@Component
export default class ToDoList extends Vue {
  mounted(): void {
    console.log("mounted");
    this.InitData();
  }
  dataLoaded = false;
  items = Array<ToDo>();
  tree = Array<TreeItem>();
  selection = Array<number>();
  active = 0;

  HasChildren(id: number): boolean {
    return this.items.find((it) => it.parentId == id) != undefined;
  }

  InitData() {
    ApiToDo.getAll().then((items) => {
      this.$data.items = items;
      this.$data.tree = arrayToTree(items, {
        childrenField: "children",
        dataField: null,
      });
      this.updateChecked(this.$data.items, true);
      this.active = 0;
      //to avoid calling watchers while initializing
      this.$nextTick(() => {
        this.dataLoaded = true;
      });
    });
  }

  updateChecked(items: Array<TreeItem>, deleteChecked = false): void {
    if (deleteChecked) {
      this.selection = [];
    }
    if (items.length == 0) return;

    for (let it of items) {
      if (it["checked"] === true) {
        this.selection.push(it["id"]);
      }
      if (Array.isArray(it["children"])) {
        this.updateChecked(it["children"]);
      }
    }
  }

  @Watch("selection")
  OnSelectionChange(newValue: Array<number>, oldValue: Array<number>) {
    if (this.dataLoaded) {
      let checked = newValue.filter((val) => !oldValue.includes(val));
      let unchecked = oldValue.filter((val) => !newValue.includes(val));

      let itemsToCheck = this.items
        .filter((it) => checked.includes(it.id))
        .map((it) => {
          it.checked = true;
          return it;
        });
      let itemsToUncheck = this.items
        .filter((it) => unchecked.includes(it.id))
        .map((it) => {
          it.checked = false;
          return it;
        });
      let itemsToUpdate = itemsToCheck.concat(itemsToUncheck);

      itemsToUpdate.forEach((element) => {
        ApiToDo.update(element).then((item) => {
          console.log("update done");
        });
      });
    }
  }

  //CRUD
  Create(parentId: number | undefined) {
    ApiToDo.update(new ToDo(0, "", parentId, false)).then((item) => {
      if (item) {
        console.log("create done");
        this.InitData();
      }
    });
  }

  ChangeName(id: number, name: string) {
    let itemToUpdate = this.items.find((it) => it.id === id);
    if (itemToUpdate) {
      itemToUpdate.name = name;
      ApiToDo.update(itemToUpdate).then((item) => {
        console.log("update done");
      });
    }
  }

  Delete(id: number) {
    let itemToDelete = this.items.find((it) => it.id === id);
    if (itemToDelete) {
      ApiToDo.delete(itemToDelete).then((item) => {
        if (item) {
          console.log("delete done");
          this.InitData();
        }
      });
    }
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
.text-crossed {
  text-decoration: line-through;
}
</style>
