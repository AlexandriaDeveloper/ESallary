export const Observer = {
  next: function(next) {
    console.log(next);
    return next;
  },
  error: function(err) {
    return err;
  },
  complete: function() {
    return true;
  }
};
