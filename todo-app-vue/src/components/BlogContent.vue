<template>
<div>
  <input type = "number" v-bind:value="post.id">
  <input type = "text" v-bind:value="post.title">
  <button v-on:click="$emit('enlarge', 0.1)">enlarge</button>
  <button v-on:click="$emit('decrease')">decrease</button>
</div>
</template>

<script>
export default {
  data() {
    return {
      count: 0
    }
  },
  props: {
    post: Object
  }
}
</script>

