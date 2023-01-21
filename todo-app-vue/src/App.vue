<template>
  <div id="app">
    <v-main>
      <ToDoList/>
      <label>input</label>
      <input type="text" v-model="input">
      <div :style="{fontSize: postFontSize + 'em'}">
      <template v-for="post in posts">
        <BlogContent :key="post.id" :post="post" v-on:enlarge="onEnlargeText"
        v-on:decrease="postFontSize = 1" />
      </template>
      </div>
    </v-main>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import ToDoList from './components/ToDoList.vue';
import BlogContent from './components/BlogContent.vue';

@Component({
  components: {
    ToDoList, BlogContent
  },
})
export default class App extends Vue {

input = "";
posts = [{id:1, title:"post 1"},{id:2, title:"post 2"}];
postFontSize = 1;

onEnlargeText(enlargeAmount: number) {
    this.postFontSize += enlargeAmount
  }


}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
