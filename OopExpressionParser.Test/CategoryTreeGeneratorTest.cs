using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Xunit;

public class CategoryTreeGeneratorTest
{
    record Category(int Id, string Name)
    {
        //fake root-category
        public static readonly Category Root = new(-1, "Root");
    }
    record SubCategory(int ParentId, int Id, string Name) : Category(Id, Name);
    record CategoryTree(Category Category, IEnumerable<CategoryTree> Children);

    // Hierarchy:
    // a:
    //  aa
    //  ab
    // b:
    //  ba
    //  bb
    readonly Category[] items =
    {
        new(1, "a"),
        new SubCategory(1, 10, "aa"),
        new SubCategory(1, 11, "ab"),

        new(2, "b"),
        new SubCategory(2, 12, "ba"),
        new SubCategory(2, 13, "bb"),
    };

    private IEnumerable<CategoryTree> CreateTree()
    {
        var lookup = items.ToLookup(it => (it as SubCategory)?.ParentId ?? Category.Root.Id); //O(n)
        var tree = CreateSubTrees(Category.Root); //O(n)
        return tree;
        
        IEnumerable<CategoryTree> CreateSubTrees(Category parent) 
            => lookup[parent.Id]
                .Select(subCat => new CategoryTree(subCat, CreateSubTrees(subCat))); 
    }
    
    [Fact]
    public void TestTree()
    {
        var tree = CreateTree();
        
        var expected = new CategoryTree[]
        {
            new(new Category(1, "a"), new[]
            {
                new SubCategory(1, 10, "aa"),
                new SubCategory(1, 11, "ab"),
            }.Select(it => new CategoryTree(it, Enumerable.Empty<CategoryTree>()))),

            new(new Category(2, "b"), new[]
            {
                new SubCategory(2, 12, "ba"),
                new SubCategory(2, 13, "bb")
            }.Select(it => new CategoryTree(it, Enumerable.Empty<CategoryTree>())))
        };

        Assert.Equal(
            JsonSerializer.Serialize(expected), 
            JsonSerializer.Serialize(tree)
        );
    }
}