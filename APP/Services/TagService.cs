using APP.Domain;
using APP.Models;
using CORE.Models;
using CORE.Services;
using Microsoft.EntityFrameworkCore;

namespace APP.Services
{
    // TagService inherits the DbService class for Tag entity type,
    // therefore Tag entity database operations of the DbService
    // class can be used in this class.
    // TagService also implements the IService interface for
    // TagRequest and TagResponse types, therefore IService method
    // definitions can be implemented for types TagRequest and
    // TagResponse in this class, which will be used by
    // the controller actions.
    public class TagService : DbService<Tag>,
        IService<TagRequest, TagResponse>
    {
        // Constructor that passes the injected DbContext instance
        // to the DbService base class constructor.
        public TagService(DbContext db) : base(db)
        {
        }

        // Include relational entities and order the query.
        protected override IQueryable<Tag> DbQuery()
        {
            return base.DbQuery()
                .Include(t => t.BlogTags)
                .ThenInclude(bt => bt.Blog)
                .OrderBy(t => t.Name);
        }

        // Project Tag entities to response models.
        public IQueryable<TagResponse> Query()
        {
            return DbQuery().Select(t => new TagResponse
            {
                Id = t.Id,
                Name = t.Name,
                BlogTitlesR = string.Join(", ", t.BlogTags
                    .OrderBy(bt => bt.Blog.Title).Select(bt => bt.Blog.Title)),
                BlogCountR = t.BlogTags.Count
            });
        }

        // Gets the Tag entity by ID then returns a mapped
        // TagRequest instance from the found Tag entity.
        public TagRequest Edit(int id)
        {
            var tagEntity = DbSingle(id);
            // Example SQL: "select * from Tags where Id = 3".

            // tagEntity may be null since the OrDefault version
            // of the Single method is used. If Single method
            // was used, an exception would be thrown.
            if (tagEntity is null) // if (tagEntity == null) can
                                    // also be written.
                return null;

            // Map each Tag entity property to TagRequest property
            // and return the TagRequest instance.
            return new TagRequest
            {
                Id = tagEntity.Id,
                Name = tagEntity.Name
            };
        }

        // Checks whether the tag with the same trimmed and
        // case sensitive name exists in the Tags database table
        // first. If it doesn't exist, inserts the Tag entity
        // mapped from the TagRequest instance to the Tags
        // database table.
        public CommandResponse Add(TagRequest request)
        {
            if (DbQuery()
                .Any(tagEntity =>
                    tagEntity.Name == request.Name.Trim()))
                return Error("\"" + request.Name + "\"" + " tag exists!");
            // Trim method removes white space characters in the
            // beginning and at the end.

            // Create a new Tag entity instance from the
            // TagRequest instance then insert the entity in the Tags
            // database table (second save default parameter of the
            // DbAdd method is true therefore changes of the DbSets
            // are commited to the related database tables).
            var tagEntity = new Tag
            {
                Name = request.Name.Trim()
            };
            DbAdd(tagEntity);

            // Entity's Id value will be updated by the database
            // after the insert operation.
            return Success(tagEntity.Id, "\"" + tagEntity.Name + "\"" + 
                " tag created successfully.");
        }

        // Checks whether the tag with the same trimmed and
        // case sensitive name other than the tag request record
        // exists in the Tags database table first.
        // If the entity doesn't exist, gets the Tag entity by ID from the
        // table, then updates the Tag entity properties
        // from tag request properties and finally updates the
        // Tag entity in the Tags database table.
        public CommandResponse Update(TagRequest request)
        {
            if (DbQuery()
                .Any(tagEntity => tagEntity.Id != request.Id &&
                    tagEntity.Name == request.Name.Trim()))
                return Error($"\"{request.Name}\" tag exists!");
            // $ with strings can be used for concatenation with { and }.
            
            // Get the Tag entity by ID from the Tags database table
            // and check whether it is null or not.
            var tagEntity = DbSingle(request.Id);
            if (tagEntity is null)
                return Error("Tag not found!");

            // Update Tag entity properties from tag request properties.
            // Then update the entity in the Tags database table (second save
            // default parameter of the DbUpdate method is true
            // therefore changes of the DbSets are commited to the related
            // database tables).
            tagEntity.Name = request.Name.Trim();
            DbUpdate(tagEntity);

            return Success(tagEntity.Id, 
                $"\"{tagEntity.Name}\" tag updated successfully.");
        }

        // Gets the Tag entity by ID from the Tags database table.
        // If the entity exists, deletes the entity from the Tags
        // database table.
        public CommandResponse Remove(int id)
        {
            // Get the Tag entity by ID from the Tags database table
            // and check whether it is null or not.
            var tagEntity = DbSingle(id);
            if (tagEntity is null)
                return Error("Tag not found!");

            // Delete relational BlogTag entities first.
            DbRemove(tagEntity.BlogTags);
            DbRemove(tagEntity);

            return Success(tagEntity.Id,
                $"\"{tagEntity.Name}\" tag deleted successfully.");
        }
    }
}
